using Fleet.Controllers.Model.Request.Abastecimento;
using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Interfaces.Service;
using Fleet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    public class AbastecimentoController(IAbastecimentoService service) : ControllerBase
    {
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]")]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromRoute] string WorkspaceId, [FromBody] AbastecimentoRequest request)
        {
            await service.Cadastrar(request,WorkspaceId);
            return Created();
        }

    }
}
