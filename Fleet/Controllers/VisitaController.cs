using Fleet.Controllers.Model.Request.Visita;
using Fleet.Controllers.Model.Response.Visita;
using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fleet.Controllers
{
    [ApiController]
    public class VisitaController(IVisitaService visitaService, IConfiguration configuration) : ControllerBase
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/[controller]")]
        public async Task<IActionResult> Criar([FromBody] CriarVisitaRequest request, [FromRoute]string WorkspaceId)
        {
            var visita = new Visitas
            {
                EstabelecimentosId = int.Parse(CriptografiaHelper.DescriptografarAes(request.EstabelecimentoId, Secret) ?? throw new BussinessException("houve uma falha em realizar a visita")),
                VeiculosId = int.Parse(CriptografiaHelper.DescriptografarAes(request.VeiculoId, Secret) ?? throw new BussinessException("houve uma falha em realizar a visita")),
                GPS = request.GPS,
                Supervior = request.Supervisor,
                Observacao = request.Observacao,
                WorkspaceId = int.Parse(CriptografiaHelper.DescriptografarAes(WorkspaceId, Secret) ?? throw new BussinessException("houve uma falha em realizar a visita")),
                Opcoes = request.Opcoes.Select(x => new VisitaOpcao
                {
                    Titulo = x.Titulo,
                    Descricao = x.Descricao,
                    Opcao = x.Opcao,
                }).ToList(),
            };

            var fotos = request.Imagens.Select(x => new Tuple<string, string, bool>(x.ImagemBase64, x.ExtensaoImagem, x.Assinatura)).ToList();

            await visitaService.Criar(visita, fotos);

            return Created();
        }

        [Authorize]
        [HttpGet("api/Workspace/{WorkspaceId}/[controller]")]
        public async Task<IActionResult> Buscar([FromRoute] string WorkspaceId)
        {
            var visitas = await visitaService.Buscar(WorkspaceId);

            return Ok(visitas.Select(x => new ListarVisitasResponse
            {
                Data = x.Data,
                Estabelecimento = x.Estabelecimentos.Fantasia,
                Observacao = x.Observacao,
            }));
        }
    }
}
