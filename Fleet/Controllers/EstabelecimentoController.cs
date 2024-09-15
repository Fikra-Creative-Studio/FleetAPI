using Fleet.Controllers.Model.Request.Estabelecimento;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [ApiController]
    public class EstabelecimentoController(IEstabelecimentoService estabelecimentoService) : ControllerBase
    {
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]")]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromRoute] string WorkspaceId, [FromBody] EstabelecimentoRequest request)
        {
            await estabelecimentoService.Cadastrar(request, WorkspaceId);
            return Created();
        }

        [HttpGet("api/Workspace/{WorkspaceId}/[Controller]")]
        [Authorize]
        public async Task<IActionResult> Listar([FromRoute] string WorkspaceId)
        {
            var estabelecimentos = await estabelecimentoService.Listar(WorkspaceId);

             return Ok(estabelecimentos);
        }
    }
}
