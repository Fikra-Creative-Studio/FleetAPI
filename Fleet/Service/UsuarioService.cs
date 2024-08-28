using AutoMapper;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Enums;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Resources;
using Fleet.Validators;

namespace Fleet.Service
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
                                IConfiguration configuration,
                                IMapper mapper,
                                IBucketService bucketService,
                                ILoggedUser loggedUser,
                                IUsuarioWorkspaceRepository usuarioWorkspaceRepository) : IUsuarioService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

        public async Task Criar(UsuarioRequest user)
        {
            Usuario usuario = mapper.Map<Usuario>(user);
            await Validar(usuario, UsuarioRequestEnum.Criar);

            await usuarioRepository.Criar(usuario);
        }

        public async Task Atualizar(UsurioPutRequest user)
        {
            var usuario = await usuarioRepository.Buscar(x => x.Id == loggedUser.UserId) ?? throw new BussinessException("Não foi possivel atualizar o usuário");
            //Validar o objeto que vindo da request

            usuario.Nome = user.Nome;
            usuario.Email = user.Email;

            await usuarioRepository.Atualizar(usuario);
        }

        public async Task UploadAsync(Stream stream, string fileExtension)
        {
            if (stream.Length > 0)
            {
                using (var file = stream)
                {
                    var filename = await bucketService.UploadAsync(stream, fileExtension) ?? throw new BussinessException("não foi possivel salvar a imagem");

                    var user = await usuarioRepository.Buscar(x => x.Id == loggedUser.UserId    ) ?? throw new BussinessException("falha para obter o usuario");
                    if (user != null && !string.IsNullOrEmpty(user.UrlImagem)) await bucketService.DeleteAsync(user.UrlImagem);

                    user.UrlImagem = filename;
                    await usuarioRepository.Atualizar(user);
                }
            }
        }

        public async Task<List<UsuarioBuscarWorkspaceResponse>> BuscarPorWorkspace(string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");

            ValidarWorkspaceAdmin(loggedUser.UserId, decryptId);

            var usuarios = await usuarioRepository.BuscarPorWorkspace(decryptId);

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

        public async Task AtualizarPapel(UsuarioAtualizarPapelRequest request)
        {
            var decryptUsuarioId = DecryptId(request.UsuarioId, "Usuario inválido");
            var decryptWorkspaceId = DecryptId(request.WorkspaceId, "Workspace inválido");
            ValidarWorkspaceAdmin(loggedUser.UserId, decryptWorkspaceId);

            if (!await usuarioWorkspaceRepository.Existe(decryptUsuarioId, decryptWorkspaceId))
                throw new BussinessException("Usuario não está vinculado a esse workspace");

            await usuarioWorkspaceRepository.AtualizarPapel(decryptUsuarioId, decryptWorkspaceId, request.Papel); 
        }

        private async void ValidarWorkspaceAdmin(int usuarioId, int workspaceId)
        {
               if (!await usuarioWorkspaceRepository.UsuarioWorkspaceAdmin(usuarioId, workspaceId)) 
                    throw new BussinessException("Usuario nao tem permissao para essa operacao"); 
        }

        private int DecryptId(string encrypt, string errorMessage)
        {
                var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
                return int.Parse(decrypt);
        }

        private async Task Validar(Usuario usuario, UsuarioRequestEnum request)
        {
            var validator = new UsuarioValidator(usuarioRepository, request);
            var validationResult = await validator.ValidateAsync(usuario);
            if (validationResult.IsValid)
            {
                usuario.Senha = CriptografiaHelper.CriptografarAes(usuario.Senha, Secret) ?? throw new BussinessException(Resource.usuario_falhaCriptografia);
            }
            else
            {
                var errors = string.Join(";", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BussinessException(errors);
            }
        }
    }
}