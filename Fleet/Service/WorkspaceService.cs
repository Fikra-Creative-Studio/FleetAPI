using System;
using AutoMapper;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Enums;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Validators;

namespace Fleet.Service;

public class WorkspaceService(ILoggedUser loggedUser,
    IWorkspaceRepository workspaceRepository, 
    IUsuarioWorkspaceRepository usuarioWorkspaceRepository, 
    IUsuarioRepository usuarioRepository, 
    IMapper mapper,
    IBucketService bucketService,
    IConfiguration configuration) : IWorskpaceService
{
    private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
    public Task Atualizar(string id)
    {
        throw new NotImplementedException();
    }

    public async Task Criar(IFormFile? file, WorkspaceRequest request)
    {
        var usuario = await usuarioRepository.Buscar(x => x.Id == loggedUser.UserId) ?? throw new BussinessException("Usuario invalido");
        var workspace = mapper.Map<Workspace>(request);

        await Validar(workspace, WorkspaceRequestEnum.Criar);

        if (file != null && file.Length > 0)
        {
            var extension = file.FileName.Split(".").Last();
            using (var stream = new MemoryStream())
            {
                Stream fileStream = stream;
                await file.CopyToAsync(fileStream);
                stream.Position = 0;
                var filename = await bucketService.UploadAsync(fileStream, extension) ?? throw new BussinessException("não foi possivel salvar a imagem");
                workspace.UrlImagem = filename;
            }
        }

        var workspaceCriado = await workspaceRepository.Criar(workspace);

        UsuarioWorkspace usuarioWorkspace = new() {
            Usuario = usuario,
            UsuarioId = usuario.Id,
            Workspace = workspaceCriado,
            WorkspaceId = workspaceCriado.Id,
            Ativo = true,
            Papel = PapelEnum.Administrador
        };
        await usuarioWorkspaceRepository.Criar(usuarioWorkspace);
    }

    public async Task<List<UsuarioBuscarWorkspaceResponse>> BuscarUsuarios(string workspaceId)
    {
        var decryptId = DecryptId(workspaceId, "Workspace inválido");

        await ValidarWorkspaceAdmin(loggedUser.UserId, decryptId);

        var usuarios = await usuarioRepository.BuscarPorWorkspace(decryptId, loggedUser.UserId);

        return usuarios.Select( x =>
            new UsuarioBuscarWorkspaceResponse {
                Id = CriptografiaHelper.CriptografarAes(x.Id.ToString(), Secret),
                CPF = x.CPF,
                Email = x.Email,
                Convidado = x.Convidado,
                Nome = x.Nome,
                UrlImagem = x.UrlImagem
            }
        ).ToList();
    }

    public async Task AtualizarPapel(string workspaceId ,WorkspaceAtualizarPermissaoRequest request)
    {
        var decryptUsuarioId = DecryptId(request.UsuarioId, "Usuario inválido");
        var decryptWorkspaceId = DecryptId(workspaceId, "Workspace inválido");
        await ValidarWorkspaceAdmin(loggedUser.UserId, decryptWorkspaceId);

        if (!await usuarioWorkspaceRepository.Existe(decryptUsuarioId, decryptWorkspaceId))
            throw new BussinessException("Usuario não está vinculado a esse workspace");

        await usuarioWorkspaceRepository.AtualizarPapel(decryptUsuarioId, decryptWorkspaceId, request.Papel); 
    }

    private async Task ValidarWorkspaceAdmin(int usuarioId, int workspaceId)
    {
        if (!await usuarioWorkspaceRepository.UsuarioWorkspaceAdmin(usuarioId, workspaceId)) 
            throw new BussinessException("Usuario nao tem permissao para essa operacao"); 
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
