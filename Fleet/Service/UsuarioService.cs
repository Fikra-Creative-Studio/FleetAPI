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
            int decryptId;
            Console.WriteLine(workspaceId);

            decryptId = int.Parse(CriptografiaHelper.DescriptografarAes(workspaceId, Secret));

            if (!await usuarioWorkspaceRepository.UsuarioWorkspaceAdmin(loggedUser.UserId, decryptId)) throw new BussinessException("Usuario nao tem permissao para essa operacao"); 

            if (decryptId == null) throw new BussinessException("Workspace inválido");
      

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
    }
}