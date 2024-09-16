using Azure.Core;
using Fleet.Controllers.Model.Request.Abastecimento;
using Fleet.Controllers.Model.Response.Veiculo;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Repository;

namespace Fleet.Service
{
    public class AbastecimentoService(
        IAbastecimentoRepository abastecimentoRepository,
        IBucketService bucketService,
        IConfiguration configuration,
        IUsuarioRepository usuarioRepository,
        ILoggedUser loggedUser,
        IVeiculoRepository veiculoRepository) : IAbastecimentoService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }  
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }
        private async Task<AbastecimentoImagens> ConverteImagens(AbastecimentoImagensRequest foto)
        {
            string NomeFoto = string.Empty;
            if (!string.IsNullOrEmpty(foto.ImagemBase64))
            {
                try
                {
                    var bytes = Convert.FromBase64String(foto.ImagemBase64);
                    NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), foto.ExtensaoImagem, "supply") ?? throw new BussinessException("não foi possivel salvar a imagem");
                }
                catch (Exception)
                {
                    NomeFoto = string.Empty;
                }
            }
            var abastecimentoImagem = new AbastecimentoImagens
            {
                Url = NomeFoto
            };
            return abastecimentoImagem;
        }


        public async Task Cadastrar(AbastecimentoRequest request, string workspaceId)
        {
            var decryptIdWorkspace = DecryptId(workspaceId, "Workspace inválido");
            var decryptIdVeiculo = DecryptId(request.VeiculosId.ToString(), "Veiculo inválido");
            var decryptIdEstabelecimento = DecryptId(request.EstabelecimentosId.ToString(), "Estabelecimento inválido");
            var usuarioLogado = await usuarioRepository.Buscar(x => x.Id == loggedUser.UserId) ?? throw new BussinessException("houve um erro na sua solicitação");

            var abastecimento = new Abastecimento
            {
                Ativo = true,
                Data = DateTime.Now,
                Odometro = request.Odometro,
                Valor = request.Valor,
                Observacoes = request.Observacoes,
                WorkspaceId = decryptIdWorkspace,
                VeiculosId = decryptIdVeiculo,
                UsuarioId = usuarioLogado.Id,
                EstabelecimentosId = decryptIdEstabelecimento,
            };

            var tasks = request.Urls.Select(ConverteImagens).ToList();
            var imagens = await Task.WhenAll(tasks);
            abastecimento.Imagens = imagens.ToList();

            await abastecimentoRepository.Cadastrar(abastecimento);
            await veiculoRepository.AtualizaOdometro(abastecimento.VeiculosId,abastecimento.Odometro);

        }
    }
}