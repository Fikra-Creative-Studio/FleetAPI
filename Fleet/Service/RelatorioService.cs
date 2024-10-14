using Fleet.Controllers.Model.Request.Relatorio;
using Fleet.Controllers.Model.Response.Relatorio;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.IdentityModel.Tokens;


namespace Fleet.Service
{
    public class RelatorioService(IRelatorioRepository relatorioRepository, IConfiguration configuration, IUsuarioRepository usuarioRepository, ILoggedUser loggedUser, IBucketService bucketService) : IRelatorioService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }
        private int DecryptIdEstabelecimento(string encrypt)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException("Estabelecimento selecionado inválido para esse relatório");
            return int.Parse(decrypt);
        }
        private int DecryptIdUsuarios(string encrypt)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException("Usuário selecionado inválido para esse relatório");
            return int.Parse(decrypt);
        }
        public async Task<string> Visitas(string workspaceId, RelatorioVisitasRequest request)
        {
            var decryptIdWorkspace = DecryptId(workspaceId, "Workspace inválido");
            //var usuarioLogado = usuarioRepository.Buscar(x => x.Id == loggedUser.UserId) ?? throw new BussinessException("houve um erro na sua solicitação");

            var visita = relatorioRepository.Listar(v => v.WorkspaceId == decryptIdWorkspace)
              .Where(x => x.Data >= request.DataInicial && x.Data <= request.DataFinal)
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
                var usuarios = request.UsuariosId.Select(DecryptIdUsuarios).ToList();
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
                var estabelecimentos = request.EstabelecimentosId.Select(DecryptIdEstabelecimento).ToList();
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
                var usuarios = request.UsuariosId.Select(DecryptIdUsuarios).ToList();
                foreach (var u in usuarios)
                {
                    var lista = respostaFull.
                    Where(x => x.Usuario.Id == u).ToList();
                    respostaUsuario.AddRange(lista);
                }

                var estabelecimentos = request.EstabelecimentosId.Select(DecryptIdEstabelecimento).ToList();
                foreach (var u in estabelecimentos)
                {
                    var lista = respostaUsuario.
                    Where(x => x.Estabelecimentos.Id == u).ToList();
                    resposta.AddRange(lista);
                }
                resposta = resposta.Distinct().ToList();

            }



            string relpath = $"{AppDomain.CurrentDomain.BaseDirectory}Service\\TemplateRelatorio\\visita.html";
            var htmlTemplate = File.ReadAllText(relpath);
            var htmlContent = htmlTemplate.Replace("{Data}", resposta[0].Data.ToString())
                                          .Replace("{Usuario.Nome}", resposta[0].Usuario.Nome)
                                          .Replace("{Veiculos.Modelo}", resposta[0].Veiculos.Modelo)
                                          .Replace("{Veiculos.Placa}", resposta[0].Veiculos.Placa);





            return "Nome do Relatório .pdf";
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
                    Workspace = relatorioRepository.BuscaWorkspace(x => x.Id == visitas.WorkspaceId).FirstOrDefault(),
                    Veiculos = relatorioRepository.BuscaVeiculo(x => x.Id == visitas.VeiculosId).FirstOrDefault(),
                    Usuario = relatorioRepository.BuscaUsuario(x => x.Id == visitas.UsuarioId).FirstOrDefault(),
                    Estabelecimentos = relatorioRepository.BuscaEstabelecimento(x => x.Id == visitas.EstabelecimentosId).FirstOrDefault(),
                    GPS = visitas.GPS,
                    Imagens = relatorioRepository.ListarImagens(x => x.VisitasId == visitas.Id).ToList(),
                    Opcoes = relatorioRepository.ListarOpcoes(x => x.VisitasId == visitas.Id).ToList()
                };
                return response;
            }
            return null;
        }

    }
}