using AutoMapper;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Enums;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Repository;
using Fleet.Resources;
using Fleet.Validators;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace Fleet.Service
{
    public class UsuarioService(IUsuarioRepository usuarioRepository,
                                IConfiguration configuration,
                                IMapper mapper,
                                IBucketService bucketService,
                                ILoggedUser loggedUser,
                                IEmailService emailService) : IUsuarioService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        public async Task Criar(UsuarioRequest user)
        {
            Usuario usuario = mapper.Map<Usuario>(user);
            usuario.Ativo = false;
            await Validar(usuario, UsuarioRequestEnum.Criar);


            var novoUsuario = usuarioRepository.Listar(x => !x.Ativo && x.Email == user.Email)
                                               .Include(x => x.UsuarioWorkspaces.Where(uw => uw.Workspace.Ativo))
                                               .ThenInclude(uw => uw.Workspace)
                                               .FirstOrDefault();
            if (novoUsuario == null)
            {
                novoUsuario = await usuarioRepository.Criar(usuario);
            }
            else
            {
                //criando a conta de um usuário inativado.
                novoUsuario.Ativo = true;
                novoUsuario.Nome = usuario.Nome;
                novoUsuario.Email = usuario.Email;
                novoUsuario.CPF = usuario.CPF;
                novoUsuario.Senha = usuario.Senha;

                foreach (var item in novoUsuario.UsuarioWorkspaces)
                {
                    item.Papel = PapelEnum.Usuario;
                }

                await usuarioRepository.Atualizar(novoUsuario);
            }


            string mailPath = $"{AppDomain.CurrentDomain.BaseDirectory}Service\\TemplateMail\\create-account.html";
            string fileContent = await File.ReadAllTextAsync(mailPath, Encoding.UTF8);
            fileContent = fileContent.Replace("{{name}}", novoUsuario.Nome)
                                     .Replace("{{link}}", $"https://juriseg.ddns.net:3307/api/Usuario/Confirmar/{CriptografiaHelper.CriptografarAes(novoUsuario.Id.ToString(), Secret)}");
            await emailService.EnviarEmail(novoUsuario.Email, novoUsuario.Nome, "Bem-vindo ao MyFleet", fileContent);
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
                    var filename = await bucketService.UploadAsync(stream, fileExtension, "profile") ?? throw new BussinessException("não foi possivel salvar a imagem");

                    var user = await usuarioRepository.Buscar(x => x.Id == loggedUser.UserId) ?? throw new BussinessException("falha para obter o usuario");
                    if (user != null && !string.IsNullOrEmpty(user.UrlImagem)) await bucketService.DeleteAsync(user.UrlImagem, "profile");

                    user.UrlImagem = filename;
                    await usuarioRepository.Atualizar(user);
                }
            }
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

        public async Task ConfirmarAsync(string id)
        {
            var idDescriptografado = int.Parse(CriptografiaHelper.DescriptografarAes(id, Secret) ?? throw new BussinessException("Não foi possivel realizar a sua operação"));
            var user = await usuarioRepository.Buscar(x => x.Id == idDescriptografado && !x.Ativo) ?? throw new BussinessException("falha para obter o usuario");
            user.Ativo = true;
            await usuarioRepository.Atualizar(user);
        }
    }
}