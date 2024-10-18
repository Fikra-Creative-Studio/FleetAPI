using Fleet.Controllers.Model.Request.Relatorio;
using Fleet.Controllers.Model.Response.Relatorio;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Fleet.Service
{
    public class RelatorioVisitaService(IRelatorioVisitaRepository relatorioRepository, IConfiguration configuration, IBucketService bucketService) : IRelatorioVisitaService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
        private int DecryptId(string encrypt)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException("Erro no filtro do relatório de visitas");
            return int.Parse(decrypt);
        }
        public async Task<string> GeraRelatorio(string workspaceId, RelatorioVisitasRequest request)
        {
            var decryptIdWorkspace = DecryptId(workspaceId);

            var visita = relatorioRepository.Listar(v => v.WorkspaceId == decryptIdWorkspace)
              .Where(x => x.Data >= request.DataInicial && x.Data <= request.DataFinal)
              .Include(v => v.Workspace)
              .Include(v => v.Veiculos)
              .Include(v => v.Usuario)
              .Include(v => v.Estabelecimentos)
              .Include(v => v.Imagens)
              .Include(v => v.Opcoes)
              .ToList();
            var respostaFull = visita.Select(ConvertVisitasToResponse).ToList();

            List<RelatorioVisitasResponse> resposta = new List<RelatorioVisitasResponse>();
            List<RelatorioVisitasResponse> respostaUsuario = new List<RelatorioVisitasResponse>();

            if (request.EstabelecimentosId.Count == 1 && request.EstabelecimentosId[0].IsNullOrEmpty()
                && request.UsuariosId.Count == 1 && request.UsuariosId[0].IsNullOrEmpty())
            {
                resposta = respostaFull;
            }
            else if (request.EstabelecimentosId.Count == 1 && request.EstabelecimentosId[0].IsNullOrEmpty())
            {
                var usuarios = request.UsuariosId.Select(DecryptId).ToList();
                foreach (var u in usuarios)
                {
                    var lista = respostaFull.
                    Where(x => x.Usuario.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();
            }
            else if(request.UsuariosId.Count == 1 && request.UsuariosId[0].IsNullOrEmpty())
            {
                var estabelecimentos = request.EstabelecimentosId.Select(DecryptId).ToList();
                foreach (var u in estabelecimentos)
                {
                    var lista = respostaFull.
                    Where(x => x.Estabelecimentos.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();
            }
            else
            {
                var usuarios = request.UsuariosId.Select(DecryptId).ToList();
                foreach (var u in usuarios)
                {
                    var lista = respostaFull.
                    Where(x => x.Usuario.Id == u).ToList();
                    respostaUsuario.AddRange(lista);
                }

                var estabelecimentos = request.EstabelecimentosId.Select(DecryptId).ToList();
                foreach (var u in estabelecimentos)
                {
                    var lista = respostaUsuario.
                    Where(x => x.Estabelecimentos.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();

            }

            var filename = await GerarRelatorioVisitaHtml(resposta, request);

            return filename;
        }

        private async Task<string> GerarRelatorioVisitaHtml(List<RelatorioVisitasResponse> relatorioVisitas, RelatorioVisitasRequest request)
        {
            string fileName ="";
            string pessimoIcone = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjYiIGhlaWdodD0iMjUiIHZpZXdCb3g9IjAgMCAyNiAyNSIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0yNS41IDEyLjVDMjUuNSAxOS40MDM2IDE5LjkwMzYgMjUgMTMgMjVDNi4wOTY0NCAyNSAwLjUgMTkuNDAzNiAwLjUgMTIuNUMwLjUgNS41OTY0NCA2LjA5NjQ0IDAgMTMgMEMxOS45MDM2IDAgMjUuNSA1LjU5NjQ0IDI1LjUgMTIuNVpNMTAuMjc1NyA5LjYxODQyQzEwLjI3NTcgMTAuNjIzOSA5LjQ2MDYgMTEuNDM5MSA4LjQ1NTA4IDExLjQzOTFDNy40NDk1NyAxMS40MzkxIDYuNjM0NDQgMTAuNjIzOSA2LjYzNDQ0IDkuNjE4NDJDNi42MzQ0NCA4LjYxMjkxIDcuNDQ5NTcgNy43OTc3OCA4LjQ1NTA4IDcuNzk3NzhDOS40NjA2IDcuNzk3NzggMTAuMjc1NyA4LjYxMjkxIDEwLjI3NTcgOS42MTg0MlpNMTcuNTQ0OSAxMS40MzkxQzE4LjU1MDQgMTEuNDM5MSAxOS4zNjU1IDEwLjYyMzkgMTkuMzY1NSA5LjYxODQyQzE5LjM2NTUgOC42MTI5MSAxOC41NTA0IDcuNzk3NzggMTcuNTQ0OSA3Ljc5Nzc4QzE2LjUzOTQgNy43OTc3OCAxNS43MjQyIDguNjEyOTEgMTUuNzI0MiA5LjYxODQyQzE1LjcyNDIgMTAuNjIzOSAxNi41Mzk0IDExLjQzOTEgMTcuNTQ0OSAxMS40MzkxWk04LjUyMTIyIDE2Ljg3M0MxMC4xODQ1IDE1LjM1NTcgMTMuODMyNCAxMy4wMDM2IDE3LjY0NjYgMTYuODk3NUMxNy45OTYyIDE3LjI1NDQgMTcuNzQxMSAxNy44NTY2IDE3LjI0MTUgMTcuODU2Nkg4LjkwMDc4QzguMzgzOTEgMTcuODU2NiA4LjEzOTM1IDE3LjIyMTMgOC41MjEyMiAxNi44NzNaIiBmaWxsPSIjMWUxZTFlIi8+Cjwvc3ZnPgo=";
            string ruimIcone = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjYiIGhlaWdodD0iMjUiIHZpZXdCb3g9IjAgMCAyNiAyNSIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0yNS43NSAxMi41QzI1Ljc1IDE5LjQwMzYgMjAuMTUzNiAyNSAxMy4yNSAyNUM2LjM0NjQ0IDI1IDAuNzUgMTkuNDAzNiAwLjc1IDEyLjVDMC43NSA1LjU5NjQ0IDYuMzQ2NDQgMCAxMy4yNSAwQzIwLjE1MzYgMCAyNS43NSA1LjU5NjQ0IDI1Ljc1IDEyLjVaTTguNzA1MDkgMTEuNDM5MUM5LjcxMDYgMTEuNDM5MSAxMC41MjU3IDEwLjYyMzkgMTAuNTI1NyA5LjYxODQyQzEwLjUyNTcgOC42MTI5MSA5LjcxMDYgNy43OTc3OCA4LjcwNTA5IDcuNzk3NzhDNy42OTk1OCA3Ljc5Nzc4IDYuODg0NDQgOC42MTI5MSA2Ljg4NDQ0IDkuNjE4NDJDNi44ODQ0NCAxMC42MjM5IDcuNjk5NTggMTEuNDM5MSA4LjcwNTA5IDExLjQzOTFaTTE3Ljc5NDkgMTEuNDM5MUMxOC44MDA0IDExLjQzOTEgMTkuNjE1NiAxMC42MjM5IDE5LjYxNTYgOS42MTg0MkMxOS42MTU2IDguNjEyOTEgMTguODAwNCA3Ljc5Nzc4IDE3Ljc5NDkgNy43OTc3OEMxNi43ODk0IDcuNzk3NzggMTUuOTc0MyA4LjYxMjkxIDE1Ljk3NDMgOS42MTg0MkMxNS45NzQzIDEwLjYyMzkgMTYuNzg5NCAxMS40MzkxIDE3Ljc5NDkgMTEuNDM5MVpNOC4xOTg3OSAxNy42MzY4QzguMzE4MDggMTcuODA5OSA4LjUxMDIyIDE3LjkwMjkgOC43MDU2NCAxNy45MDI5QzguODI1OTIgMTcuOTAyOSA4Ljk0NzUyIDE3Ljg2NzYgOS4wNTQwOSAxNy43OTQyQzkuMDY0NDggMTcuNzg3IDEwLjExMTggMTcuMDczNiAxMS42ODM0IDE2LjcxMjZDMTIuNiAxNi41MDIgMTMuNTA1IDE2LjQ1NTkgMTQuMzczMyAxNi41NzU2QzE1LjQ1NTMgMTYuNzI0OCAxNi40ODkxIDE3LjEzNDggMTcuNDQ2IDE3Ljc5NDJDMTcuNzI1NiAxNy45ODcgMTguMTA4NSAxNy45MTY1IDE4LjMwMTMgMTcuNjM2OEMxOC40OTQgMTcuMzU3MiAxOC40MjM1IDE2Ljk3NDMgMTguMTQzOSAxNi43ODE2QzE2LjEzOTggMTUuNDAwNCAxMy44MTA1IDE0Ljk2MiAxMS40MDggMTUuNTE0QzkuNjA5NzkgMTUuOTI3MSA4LjQwNjUxIDE2Ljc0NjkgOC4zNTYxNSAxNi43ODE2QzguMDc2NTEgMTYuOTc0MyA4LjAwNjA3IDE3LjM1NzIgOC4xOTg3OSAxNy42MzY4WiIgZmlsbD0iIzFlMWUxZSIvPgo8L3N2Zz4K";
            string regularIcone = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjUiIGhlaWdodD0iMjUiIHZpZXdCb3g9IjAgMCAyNSAyNSIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0yNSAxMi41QzI1IDE5LjQwMzYgMTkuNDAzNiAyNSAxMi41IDI1QzUuNTk2NDQgMjUgMCAxOS40MDM2IDAgMTIuNUMwIDUuNTk2NDQgNS41OTY0NCAwIDEyLjUgMEMxOS40MDM2IDAgMjUgNS41OTY0NCAyNSAxMi41Wk03LjcxMTM3IDExLjQzOTFDOC43MTY4OCAxMS40MzkxIDkuNTMyMDEgMTAuNjIzOSA5LjUzMjAxIDkuNjE4NDJDOS41MzIwMSA4LjYxMjkxIDguNzE2ODggNy43OTc3OCA3LjcxMTM3IDcuNzk3NzhDNi43MDU4NSA3Ljc5Nzc4IDUuODkwNzIgOC42MTI5MSA1Ljg5MDcyIDkuNjE4NDJDNS44OTA3MiAxMC42MjM5IDYuNzA1ODUgMTEuNDM5MSA3LjcxMTM3IDExLjQzOTFaTTE2LjgwMTIgMTEuNDM5MUMxNy44MDY3IDExLjQzOTEgMTguNjIxOCAxMC42MjM5IDE4LjYyMTggOS42MTg0MkMxOC42MjE4IDguNjEyOTEgMTcuODA2NyA3Ljc5Nzc4IDE2LjgwMTIgNy43OTc3OEMxNS43OTU3IDcuNzk3NzggMTQuOTgwNSA4LjYxMjkxIDE0Ljk4MDUgOS42MTg0MkMxNC45ODA1IDEwLjYyMzkgMTUuNzk1NyAxMS40MzkxIDE2LjgwMTIgMTEuNDM5MVpNNy4zNDAxNyAxNy4wMDkxQzcuMzQwMTcgMTcuMzQ4OCA3LjYxNTQ3IDE3LjYyNDEgNy45NTUwOSAxNy42MjQxSDE3LjA0NDlDMTcuMzg0NSAxNy42MjQxIDE3LjY1OTggMTcuMzQ4OCAxNy42NTk4IDE3LjAwOTFDMTcuNjU5OCAxNi42Njk1IDE3LjM4NDUgMTYuMzk0MiAxNy4wNDQ5IDE2LjM5NDJINy45NTUwOUM3LjYxNTQ3IDE2LjM5NDIgNy4zNDAxNyAxNi42Njk1IDcuMzQwMTcgMTcuMDA5MVoiIGZpbGw9IiMxZTFlMWUiLz4KPC9zdmc+Cg==";
            string bomIcone = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjYiIGhlaWdodD0iMjUiIHZpZXdCb3g9IjAgMCAyNiAyNSIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0yNS4yNSAxMi41QzI1LjI1IDE5LjQwMzYgMTkuNjUzNiAyNSAxMi43NSAyNUM1Ljg0NjQ0IDI1IDAuMjUgMTkuNDAzNiAwLjI1IDEyLjVDMC4yNSA1LjU5NjQ0IDUuODQ2NDQgMCAxMi43NSAwQzE5LjY1MzYgMCAyNS4yNSA1LjU5NjQ0IDI1LjI1IDEyLjVaTTcuOTYxMzYgMTEuNDM5MUM4Ljk2Njg4IDExLjQzOTEgOS43ODIwMSAxMC42MjM5IDkuNzgyMDEgOS42MTg0MkM5Ljc4MjAxIDguNjEyOTEgOC45NjY4OCA3Ljc5Nzc4IDcuOTYxMzYgNy43OTc3OEM2Ljk1NTg1IDcuNzk3NzggNi4xNDA3MiA4LjYxMjkxIDYuMTQwNzIgOS42MTg0MkM2LjE0MDcyIDEwLjYyMzkgNi45NTU4NSAxMS40MzkxIDcuOTYxMzYgMTEuNDM5MVpNMTcuMDUxMiAxMS40MzkxQzE4LjA1NjcgMTEuNDM5MSAxOC44NzE4IDEwLjYyMzkgMTguODcxOCA5LjYxODQyQzE4Ljg3MTggOC42MTI5MSAxOC4wNTY3IDcuNzk3NzggMTcuMDUxMiA3Ljc5Nzc4QzE2LjA0NTYgNy43OTc3OCAxNS4yMzA1IDguNjEyOTEgMTUuMjMwNSA5LjYxODQyQzE1LjIzMDUgMTAuNjIzOSAxNi4wNDU2IDExLjQzOTEgMTcuMDUxMiAxMS40MzkxWk0xNy44MDEzIDE1LjUzNDFDMTcuNjgxOSAxNS4zNjEgMTcuNDg5OCAxNS4yNjgxIDE3LjI5NDQgMTUuMjY4MUMxNy4xNzQxIDE1LjI2ODEgMTcuMDUyNSAxNS4zMDMzIDE2Ljk0NTkgMTUuMzc2N0MxNi45MzU1IDE1LjM4MzkgMTUuODg4MyAxNi4wOTczIDE0LjMxNjYgMTYuNDU4M0MxMy40IDE2LjY2ODkgMTIuNDk1MSAxNi43MTUgMTEuNjI2NyAxNi41OTUzQzEwLjU0NDcgMTYuNDQ2MiA5LjUxMDkyIDE2LjAzNjIgOC41NTQwNiAxNS4zNzY3QzguMjc0NDIgMTUuMTg0IDcuODkxNTEgMTUuMjU0NSA3LjY5ODc5IDE1LjUzNDFDNy41MDYwNyAxNS44MTM3IDcuNTc2NTEgMTYuMTk2NiA3Ljg1NjE1IDE2LjM4OTNDOS44NjAyOSAxNy43NzA2IDEyLjE4OTUgMTguMjA4OSAxNC41OTIgMTcuNjU3QzE2LjM5MDIgMTcuMjQzOCAxNy41OTM1IDE2LjQyNDEgMTcuNjQzOSAxNi4zODkzQzE3LjkyMzUgMTYuMTk2NiAxNy45OTQgMTUuODEzNyAxNy44MDEzIDE1LjUzNDFaIiBmaWxsPSIjMWUxZTFlIi8+Cjwvc3ZnPgo=";
            string otimoIcone = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjYiIGhlaWdodD0iMjUiIHZpZXdCb3g9IjAgMCAyNiAyNSIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0yNS41IDEyLjVDMjUuNSAxOS40MDM2IDE5LjkwMzYgMjUgMTMgMjVDNi4wOTY0MyAyNSAwLjUgMTkuNDAzNiAwLjUgMTIuNUMwLjUgNS41OTY0NCA2LjA5NjQzIDAgMTMgMEMxOS45MDM2IDAgMjUuNSA1LjU5NjQ0IDI1LjUgMTIuNVpNMTAuMDMyIDkuNjE4NDJDMTAuMDMyIDEwLjYyMzkgOS4yMTY4MyAxMS40MzkxIDguMjExMzIgMTEuNDM5MUM3LjIwNTggMTEuNDM5MSA2LjM5MDY4IDEwLjYyMzkgNi4zOTA2OCA5LjYxODQyQzYuMzkwNjggOC42MTI5MSA3LjIwNTggNy43OTc3OCA4LjIxMTMyIDcuNzk3NzhDOS4yMTY4MyA3Ljc5Nzc4IDEwLjAzMiA4LjYxMjkxIDEwLjAzMiA5LjYxODQyWk0xNy4zMDEyIDExLjQzOTFDMTguMzA2NyAxMS40MzkxIDE5LjEyMTggMTAuNjIzOSAxOS4xMjE4IDkuNjE4NDJDMTkuMTIxOCA4LjYxMjkxIDE4LjMwNjcgNy43OTc3OCAxNy4zMDEyIDcuNzk3NzhDMTYuMjk1NiA3Ljc5Nzc4IDE1LjQ4MDUgOC42MTI5MSAxNS40ODA1IDkuNjE4NDJDMTUuNDgwNSAxMC42MjM5IDE2LjI5NTYgMTEuNDM5MSAxNy4zMDEyIDExLjQzOTFaTTE3LjM3ODMgMTUuNzUzMkMxNS43MTUgMTcuMjcwNCAxMi4wNjcyIDE5LjYyMjUgOC4yNTI5NSAxNS43Mjg2QzcuOTAzMzQgMTUuMzcxNyA4LjE1ODQ0IDE0Ljc2OTUgOC42NTgwNCAxNC43Njk1SDE2Ljk5ODdDMTcuNTE1NiAxNC43Njk1IDE3Ljc2MDIgMTUuNDA0OCAxNy4zNzgzIDE1Ljc1MzJaIiBmaWxsPSIjMWUxZTFlIi8+Cjwvc3ZnPgo=";
            string imagesHtml = "";
            string opcoesHtml = "";
            string imagemAssinatura = "";
            string templaterelatorio = "";
            string visitapath = $"{AppDomain.CurrentDomain.BaseDirectory}Service/TemplateRelatorio/visita.html";
            string relpath = $"{AppDomain.CurrentDomain.BaseDirectory}Service/TemplateRelatorio/capa.html";
            string html = File.ReadAllText(relpath);
            string data = "";
            bool inserirData;
          
            foreach(var relatorioVisita in relatorioVisitas) {
                foreach (var opcao in relatorioVisita.Opcoes) {
                    string[] classes = ["", "", "", "", ""];
                    if (opcao.Opcao >= 1 && opcao.Opcao <= 5)
                    {
                        classes[opcao.Opcao - 1] = "active";
                    }
                    opcoesHtml += $@"
                        <tr>
                            <td>
                                <p><strong>{opcao.Titulo}</strong> {opcao.Descricao}</p>
                            </td>
                            <td><img src='{pessimoIcone}' alt='' class='{classes[0]}'></td>
                            <td><img src='{ruimIcone}' alt='' class='{classes[1]}'></td>
                            <td><img src='{regularIcone}' alt='' class='{classes[2]}'></td>
                            <td><img src='{bomIcone}' alt='' class='{classes[3]}'></td>
                            <td><img src='{otimoIcone}' alt='' class='{classes[4]}'></td>
                        </tr>
                    ";
                }
                foreach (var imagem in relatorioVisita.Imagens) {
                    if (!imagem.FotoAssinatura)
                        imagesHtml += $@"<img src='../visit/{imagem.Url}'>";
                    else
                        imagemAssinatura = $"../signature/{imagem.Url}";
                }

                string endereco = $"Endereço: {relatorioVisita.Estabelecimentos.Rua}, {relatorioVisita.Estabelecimentos.Numero} - {relatorioVisita.Estabelecimentos.Bairro}, {relatorioVisita.Estabelecimentos.Cidade}, {relatorioVisita.Estabelecimentos.Cep}";
                string veiculo = $"{relatorioVisita.Veiculos.Marca} {relatorioVisita.Veiculos.Modelo}, {relatorioVisita.Veiculos.Combustivel}, {relatorioVisita.Veiculos.Placa}, {relatorioVisita.Veiculos.Odometro} KM";
                string maps = $"https://www.google.com/maps?q={relatorioVisita.GPS.Replace(" ", "")}&z=13&output=embed";
                var bodyHtml = File.ReadAllText(visitapath);
                if (data == "" || data != relatorioVisita.Data.ToString("dd.MM.yyyy")) {
                    data = relatorioVisita.Data.ToString("dd.MM.yyyy");
                    inserirData = true;
                } else {
                    inserirData = false;
                }
                    

                templaterelatorio += bodyHtml.Replace("{{data_visita}}", inserirData ? data : "")
                                        .Replace("{{empresa_visita}}", relatorioVisita.Estabelecimentos.Fantasia)
                                        .Replace("{{endereco_visita}}", endereco)
                                        .Replace("{{visitante}}", relatorioVisita.Usuario.Nome)
                                        .Replace("{{veiculo_visita}}", veiculo)
                                        .Replace("{{tabela_relatorio}}", opcoesHtml)
                                        .Replace("{{tabela_relatorio}}", relatorioVisita.Observacao)
                                        .Replace("{{sing_image}}", imagemAssinatura)
                                        .Replace("{{images}}", imagesHtml)
                                        .Replace("{{responsavel}}", relatorioVisita.Supervior)
                                        .Replace("{{observacao}}", relatorioVisita.Observacao)
                                        .Replace("{{maps}}", maps);
               
                imagesHtml = "";
                opcoesHtml = "";
                imagemAssinatura = "";
            }

            var relatorio = html.Replace("{{relatorio}}", "Visita")
                                .Replace("{{data_inicio}}", request.DataInicial.ToString("dd.MM.yyyy"))
                                .Replace("{{data_fim}}", request.DataFinal.ToString("dd.MM.yyyy"))
                                .Replace("{{relatorio_template}}",templaterelatorio);

            
            var htmlContent = relatorio.ToString();

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    await writer.WriteAsync(htmlContent);
                    await writer.FlushAsync();
                }

                stream.Position = 0;

                fileName = await bucketService.UploadAsync(stream, "html", "report");
            }

            return fileName;
        }

        private RelatorioVisitasResponse ConvertVisitasToResponse(Visitas visitas)
        {
            if (visitas != null)
            {
                RelatorioVisitasResponse response = new RelatorioVisitasResponse
                {
                    Id = visitas.Id,
                    Data = visitas.Data,
                    Observacao = visitas.Observacao,
                    Supervior = visitas.Supervior,
                    Workspace = visitas.Workspace,
                    Veiculos = visitas.Veiculos,
                    Usuario = visitas.Usuario,
                    Estabelecimentos = visitas.Estabelecimentos,
                    GPS = visitas.GPS,
                    Imagens = visitas.Imagens,
                    Opcoes = visitas.Opcoes
                };
                return response;
            }
            return null;
        }

    }
}