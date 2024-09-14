using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Interfaces.Service;
using Fleet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController(IVeiculoService veiculoService) : ControllerBase
    {
        [HttpPost("{WorkspaceId}/Cadastrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar([FromRoute] int WorkspaceId, [FromBody] VeiculoRequest request)
        {
            await veiculoService.Cadastrar(request, WorkspaceId);
            return Created();
        }

        [HttpGet("{WorkspaceId}/Listar")]
        [AllowAnonymous]
        public async Task<IActionResult> Listar([FromRoute] int WorkspaceId)
        {
            var veiculos = await veiculoService.Listar(WorkspaceId);

            return Ok(veiculos);
        }


    }
}
