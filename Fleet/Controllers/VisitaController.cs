using Fleet.Controllers.Model.Request.Visita;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitaController(IVisitaService visitaService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarVisitaRequest request)
        {
            await visitaService.Criar(request);

            return Created();
        }

        [Authorize]
        [HttpGet("[Action]/{WorkspaceId}")]
        public async Task<IActionResult> Buscar([FromRoute] string WorkspaceId)
        {
            var visitas = visitaService.Buscar(WorkspaceId);

            return Ok(visitas);
        }
    }
}
