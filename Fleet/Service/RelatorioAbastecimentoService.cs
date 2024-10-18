using Fleet.Controllers.Model.Request.Relatorio;
using Fleet.Controllers.Model.Response.Relatorio;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Fleet.Service
{
    public class RelatorioAbastecimentoService(IRelatorioAbastecimentoRepository relatorioRepository,IConfiguration configuration, IBucketService bucketService): IRelatorioAbastecimentoService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
        private int DecryptId(string encrypt)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException("Erro no filtro do relatório de Abastecimento");
            return int.Parse(decrypt);
        }

        public async Task<string> GeraRelatorio(string workspaceId, RelatorioAbastecimentoRequest request)
        {
            var decryptIdWorkspace = DecryptId(workspaceId);

            var abastecimento =relatorioRepository.Listar(v => v.WorkspaceId == decryptIdWorkspace)
              .Where(x => x.Data >= request.DataInicial && x.Data <= request.DataFinal)
              .Include(v => v.Workspace)
              .Include(v => v.Veiculos)
              .Include(v => v.Usuario)
              .Include(v => v.Estabelecimentos)
              .Include(v => v.Imagens)
              .ToList();

            List<Abastecimento> resposta = new List<Abastecimento>();
            List<Abastecimento> respostaVeiculo = new List<Abastecimento>();

            if (request.EstabelecimentosId.Count == 1 && request.EstabelecimentosId[0].IsNullOrEmpty()
                && request.VeiculoId.Count == 1 && request.VeiculoId[0].IsNullOrEmpty())
            {
                resposta = abastecimento;
            }
            else if (request.EstabelecimentosId.Count == 1 && request.EstabelecimentosId[0].IsNullOrEmpty())
            {
                var veiculos = request.VeiculoId.Select(DecryptId).ToList();
                foreach (var u in veiculos)
                {
                    var lista = abastecimento.
                    Where(x => x.Veiculos.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();
            }
            else if(request.VeiculoId.Count == 1 && request.VeiculoId[0].IsNullOrEmpty())
            {
                var estabelecimentos = request.EstabelecimentosId.Select(DecryptId).ToList();
                foreach (var u in estabelecimentos)
                {
                    var lista = abastecimento.
                    Where(x => x.Estabelecimentos.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();
            }
            else
            {
                var veiculos = request.VeiculoId.Select(DecryptId).ToList();
                foreach (var u in veiculos)
                {
                    var lista = abastecimento.
                    Where(x => x.Usuario.Id == u).ToList();
                    respostaVeiculo.AddRange(lista);
                }

                var estabelecimentos = request.EstabelecimentosId.Select(DecryptId).ToList();
                foreach (var u in estabelecimentos)
                {
                    var lista = respostaVeiculo.
                    Where(x => x.Estabelecimentos.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();

            }


            

            return "Nome do relatório";
        }
    }
}
