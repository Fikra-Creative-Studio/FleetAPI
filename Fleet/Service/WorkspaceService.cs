using AutoMapper;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Enums;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Validators;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Fleet.Service;

public class WorkspaceService(ILoggedUser loggedUser,
    IWorkspaceRepository workspaceRepository, 
    IUsuarioWorkspaceRepository usuarioWorkspaceRepository, 
    IUsuarioRepository usuarioRepository, 
    IMapper mapper,
    IBucketService bucketService,
    IConfiguration configuration,
    IEmailService emailService) : IWorskpaceService
{
    private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
    public Task Atualizar(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Workspace> Criar(WorkspaceRequest request)
    {
        if(await workspaceRepository.ExisteCnpj(request.CNPJ)) throw new BussinessException("CNPJ já cadastrado");

        string NomeFoto = string.Empty;
        if (!string.IsNullOrEmpty(request.ImagemBase64))
        {
            try
            {
                var bytes = Convert.FromBase64String(request.ImagemBase64);
                NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), request.ExtensaoImagem, "workspace") ?? throw new BussinessException("não foi possivel salvar a imagem");
            }
            catch (Exception)
            {
                NomeFoto = string.Empty;
            }
        }

        var workspace = new Workspace
        {
            Ativo = true,
            Cnpj = request.CNPJ,
            Fantasia = request.Fantasia,
            UrlImagem = NomeFoto,
            UsuarioWorkspaces = new List<UsuarioWorkspace>()
            {
                new UsuarioWorkspace
                {
                    Ativo= true,
                    Papel = PapelEnum.Administrador,
                    UsuarioId = loggedUser.UserId,
                }
            }
        };

        return await workspaceRepository.Criar(workspace);
    }

    public async Task<List<UsuarioBuscarWorkspaceResponse>> BuscarUsuarios(string workspaceId)
    {
        var decryptId = DecryptId(workspaceId, "Workspace inválido");

        await ValidarWorkspaceAdmin(loggedUser.UserId, decryptId);

        var usuarios = usuarioRepository.Listar(u => u.UsuarioWorkspaces.Any(uw => uw.WorkspaceId == decryptId) && u.Id != loggedUser.UserId)
                                        .Include(u => u.UsuarioWorkspaces.Where(uw => uw.WorkspaceId == decryptId))
                                            .ThenInclude(uw => uw.Workspace)
                                        .ToList();

        return usuarios.Select( x =>
            new UsuarioBuscarWorkspaceResponse {
                Id = CriptografiaHelper.CriptografarAes(x.Id.ToString(), Secret),
                CPF = x.CPF,
                Email = x.Email,
                Nome = x.Nome,
                UrlImagem = x.UrlImagem,
                Papel = x.UsuarioWorkspaces.Where(uw => uw.WorkspaceId == decryptId)
                                        .Select(uw => uw.Papel) // Seleciona o campo Papel
                                        .FirstOrDefault()
            }
        ).ToList();
    }

    public async Task AtualizarPapel(string workspaceId ,WorkspaceAtualizarPermissaoRequest request)
    {
        var decryptUsuarioId = DecryptId(request.UsuarioId, "Usuario inválido");
        var decryptWorkspaceId = DecryptId(workspaceId, "Workspace inválido");
        await ValidarWorkspaceAdmin(loggedUser.UserId, decryptWorkspaceId);

        if (!await usuarioWorkspaceRepository.Existe(x => x.UsuarioId == decryptUsuarioId && x.WorkspaceId == decryptWorkspaceId))
            throw new BussinessException("Usuario não está vinculado a esse workspace");

        await usuarioWorkspaceRepository.AtualizarPapel(decryptUsuarioId, decryptWorkspaceId, request.Papel); 
    }

    public async Task ConvidarUsuario(string workspaceId, string email)
    {
        var decryptWorkspaceId = DecryptId(workspaceId, "Workspace inválido");
        var workspace = await workspaceRepository.Buscar(x => x.Id == decryptWorkspaceId) ?? throw new BussinessException("Id do Workspace não encontrado");

        await ValidarWorkspaceAdmin(loggedUser.UserId, decryptWorkspaceId);

        var usuario = await usuarioRepository.Buscar(x => x.Email == email);
       
        string message = $"Você foi chamado para o ambiente do {workspace.Fantasia}";
        
        if (usuario == null) {
            var password = PasswordGeneratorHelper.GenerateRandomPassword();
            Usuario novoUsuario = new() {
                Email = email,
                Senha = CriptografiaHelper.CriptografarAes(password, Secret)
            };

            await usuarioRepository.Criar(novoUsuario);
            usuario = novoUsuario;
            message += $"\nPara seu primeiro acesso use seu e-mail e a senha {password}";
        }

        UsuarioWorkspace usuarioWorkspace = new() {
            UsuarioId = usuario.Id,
            Usuario = usuario,
            WorkspaceId = workspace.Id,
            Workspace = workspace,
            Ativo = true,
            Papel = PapelEnum.Convidado
        };

        await usuarioWorkspaceRepository.Criar(usuarioWorkspace);

        await emailService.EnviarEmail(usuario.Email, usuario.Nome != null && usuario.Nome != "" ? usuario.Nome : "Convidado", "Convite para ambiente", message);
    }

    public async Task RemoverUsuario(string workspaceId, string usuarioId)
    {
        var decryptWorkspaceId = DecryptId(workspaceId, "Workspace inválido");

        await ValidarWorkspaceAdmin(loggedUser.UserId, decryptWorkspaceId);

        var decryptUsuarioId = DecryptId(usuarioId, "Usuario inválido");

        if(!await usuarioWorkspaceRepository.Existe(x => x.UsuarioId == decryptUsuarioId && x.WorkspaceId == decryptWorkspaceId)) 
            throw new BussinessException("Usuário não encontrado nesse workspace");

        if (await UsuarioUnicoAdmin(decryptUsuarioId, decryptWorkspaceId))
            throw new BussinessException("Usuario não pode ser removido pois é o único administrador desse workspace");
        
        await usuarioWorkspaceRepository.Remover(decryptUsuarioId, decryptWorkspaceId);
    }

    private async Task ValidarWorkspaceAdmin(int usuarioId, int workspaceId)
    {
        if (!await UsuarioAdmin(usuarioId, workspaceId)) 
            throw new BussinessException("Usuario nao tem permissao para essa operacao"); 
    }

    private async Task<bool> UsuarioUnicoAdmin(int usuarioId, int workspaceId)
    {
        if (!await UsuarioAdmin(usuarioId, workspaceId)) return false;
        return !await usuarioWorkspaceRepository.Existe(uw => uw.UsuarioId != usuarioId && 
                                            uw.WorkspaceId == workspaceId && 
                                            uw.Papel == PapelEnum.Administrador);
    }

    private async Task<bool> UsuarioAdmin(int usuarioId, int workspaceId)
    {
        return await usuarioWorkspaceRepository.Existe(uw => uw.UsuarioId == usuarioId && 
                                            uw.WorkspaceId == workspaceId && 
                                            uw.Papel == PapelEnum.Administrador);
    }

    private int DecryptId(string encrypt, string errorMessage)
    {
        var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
        return int.Parse(decrypt);
    }

    private async Task Validar(Workspace workspace, WorkspaceRequestEnum request)
    {
        var validator = new WorkspaceValidator(workspaceRepository, request);
        var validationResult = await validator.ValidateAsync(workspace);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(";", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BussinessException(errors);
        }
    }


}
