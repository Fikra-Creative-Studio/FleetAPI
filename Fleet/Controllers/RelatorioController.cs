using Fleet.Controllers.Model.Request.Relatorio;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [ApiController]
    public class RelatorioController(IRelatorioService service) : ControllerBase
    {
        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]")]
        public async Task<IActionResult> Visitas([FromRoute] string WorkspaceId, [FromBody] RelatorioVisitasRequest request)
        {
            var resposta = await service.Visitas(WorkspaceId,request);
            return Ok(resposta);
        }
    }
}