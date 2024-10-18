using Fleet.Controllers.Model.Request.Relatorio;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [ApiController]
    public class RelatorioController(IRelatorioVisitaService serviceVisitas, IRelatorioAbastecimentoService serviceAbastecimento) : ControllerBase
    {
        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]/Visitas")]
        public async Task<IActionResult> Visitas([FromRoute] string WorkspaceId, [FromBody] RelatorioVisitasRequest request)
        {
            var resposta = await serviceVisitas.GeraRelatorio(WorkspaceId,request);
            return Ok(resposta);
        }

        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]/Abastecimento")]
        public async Task<IActionResult> Abastecimento([FromRoute] string WorkspaceId, [FromBody] RelatorioAbastecimentoRequest request)
        {
            var resposta = await serviceAbastecimento.GeraRelatorio(WorkspaceId, request);
            return Ok(resposta);
        }
    }
}