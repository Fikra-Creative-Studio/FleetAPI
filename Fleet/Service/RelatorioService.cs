using Fleet.Controllers.Model.Request.Relatorio;
using Fleet.Controllers.Model.Response.Relatorio;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq;
using System.Security.Cryptography;

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
            
            //verificar se foi passado algum estabelecimento ou usuario, caso não, listar tudo no período
            var estabelecimentos = request.EstabelecimentosId.Select(DecryptIdEstabelecimento).ToList();
            var usuarios = request.UsuariosId.Select(DecryptIdUsuarios).ToList();
            var visita = relatorioRepository.Listar(v => v.WorkspaceId == decryptIdWorkspace)
                .Where(x => x.Data >= request.DataInicial && x.Data <= request.DataFinal)
                .ToList();


            var respostaFull = visita.Select(ConvertVisitasToResponse).ToList();

            List<RelatorioVisitasResponse> respostaUsuario = new List<RelatorioVisitasResponse>();

            foreach (var u in usuarios)
            {
                var lista = respostaFull.
                Where(x => x.Usuario.Id == u).ToList();
                respostaUsuario.AddRange(lista);
            }
            respostaUsuario = respostaUsuario.Distinct().ToList();

            List<RelatorioVisitasResponse> respostaEstabelecimento = new List<RelatorioVisitasResponse>();

            foreach (var u in estabelecimentos)
            {
                var lista = respostaFull.
                Where(x => x.Estabelecimentos.Id == u).ToList();
                respostaEstabelecimento.AddRange(lista);
            }
            respostaEstabelecimento = respostaEstabelecimento.Distinct().ToList();




            var NomeArquivo = string.Empty;
            //if (respostaFiltro != null)
            //{
            //    try
            //    {
            //        var bytes = Convert.FromBase64String(respostaFiltro.First().Observacao);
            //        NomeArquivo = await bucketService.UploadAsync(new MemoryStream(bytes), "pdf", "report") ?? throw new BussinessException("não foi possivel salvar o relatório");
            //    }
            //    catch (Exception)
            //    {
            //        NomeArquivo = string.Empty;
            //    }
            //}

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