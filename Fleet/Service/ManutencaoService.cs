using Fleet.Controllers.Model.Request.Manutencao;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class ManutencaoService(
        IManutencaoRepository manutencaoRepository,
        IBucketService bucketService,
        IConfiguration configuration,
        IUsuarioRepository usuarioRepository,
        ILoggedUser loggedUser,
        IVeiculoRepository veiculoRepository) : IManutencaoService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }
        private async Task<ManutencaoImagens> ConverteImagens(ManutencaoIMagensRequest foto)
        {
            string NomeFoto = string.Empty;
            if (!string.IsNullOrEmpty(foto.ImagemBase64))
            {
                try
                {
                    var bytes = Convert.FromBase64String(foto.ImagemBase64);
                    NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), foto.ExtensaoImagem, "maintenance") ?? throw new BussinessException("não foi possivel salvar a imagem");
                }
                catch (Exception)
                {
                    NomeFoto = string.Empty;
                }
            }
            var manutencaoImagem = new ManutencaoImagens
            {
                Url = NomeFoto
            };
            return manutencaoImagem;
        }


        public async Task Cadastrar(ManutencaoRequest request, string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            var usuarioLogado = await usuarioRepository.Buscar(x => x.Id == loggedUser.UserId) ?? throw new BussinessException("houve um erro na sua solicitação");

            var manutencao = new Manutencao
            {
                Ativo = true,
                Data = DateTime.Now,
                Odometro = request.Odometro,
                Valor = request.Valor,
                Servicos = request.Servicos,
                Observacoes = request.Observacoes,
                WorkspaceId = decryptId,
                VeiculosId = request.VeiculosId,
                UsuarioId = usuarioLogado.Id,
                EstabelecimentosId = request.EstabelecimentosId,
            };

            var tasks = request.Urls.Select(ConverteImagens).ToList();
            var imagens = await Task.WhenAll(tasks);
            manutencao.Imagens = imagens.ToList();

            await manutencaoRepository.Cadastrar(manutencao);
            await veiculoRepository.AtualizaOdometro(manutencao.VeiculosId, manutencao.Odometro);

        }
    }
}
