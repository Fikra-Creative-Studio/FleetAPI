using Fleet.Controllers.Model.Request.Manutencao;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    public class ManutencaoController(IManutencaoService service) : ControllerBase
    {
       
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]")]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromRoute] string WorkspaceId, [FromBody] ManutencaoRequest request)
        {
            await service.Cadastrar(request, WorkspaceId);
            return Created();
        }

    }
}
