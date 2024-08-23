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
using Microsoft.Extensions.Configuration;

namespace Fleet.Service
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
                                IConfiguration configuration,
                                IMapper mapper,
                                IBucketService bucketService) : IUsuarioService
    {

        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

        public async Task Criar(UsuarioRequest user)
        {
            Usuario usuario = mapper.Map<Usuario>(user);
            await Validar(usuario, UsuarioRequestEnum.Criar);

            await usuarioRepository.Criar(usuario);
        }

         public async Task Atualizar(string id, UsurioPutRequest user)
        {
            var usuarioId = int.Parse(CriptografiaHelper.DescriptografarAes(id, Secret));
            var usuario = await usuarioRepository.Buscar(x => x.Id == usuarioId);

            if(usuario == null) throw new BussinessException("Não foi possivel atualizar o usuário");
            //Validar o objeto que vindo da request

            usuario.Nome = user.Nome;
            usuario.Email = user.Email;

            await usuarioRepository.Atualizar(usuario);
        }

        public async Task Deletar(string id)
        {
            var usuario = new Usuario {
                Id = int.Parse(CriptografiaHelper.DescriptografarAes(id, Secret))
            };
            await Validar(usuario, UsuarioRequestEnum.Deletar);

            await usuarioRepository.Deletar(usuario.Id);
        }

        public async Task<List<UsuarioResponse>> Listar()
        {
            var usuarios =  await usuarioRepository.Listar();

            List<UsuarioResponse> usuariosResponse = [];

            foreach(var usuario in usuarios) {
                usuariosResponse.Add(new UsuarioResponse(
                                    CriptografiaHelper.CriptografarAes(usuario.Id.ToString(), Secret), 
                                    usuario.Nome, 
                                    usuario.CPF, 
                                    usuario.Email, 
                                    usuario.UrlImagem));
            }
            return usuariosResponse;
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

        public async Task UploadAsync(string id, Stream stream, string fileExtension)
        {
            if (stream.Length > 0)
            {
                var secretCrypto = configuration.GetValue<string>("Crypto:Secret") ?? throw new BussinessException("falha em criptografia");
                var userId = CriptografiaHelper.DescriptografarAes(id, secretCrypto) ?? throw new BussinessException("falha para obter o usuario");

                var filename = await bucketService.UploadAsync(stream, fileExtension) ?? throw new BussinessException("não foi possivel salvar a imagem");

                var user = await usuarioRepository.Buscar(x => x.Id == int.Parse(userId)) ?? throw new BussinessException("falha para obter o usuario");
                if(user != null &&! string.IsNullOrEmpty(user.UrlImagem)) await bucketService.DeleteAsync(user.UrlImagem);

                user.UrlImagem = filename;
                await usuarioRepository.Atualizar(user);
            }
        }
    }
}