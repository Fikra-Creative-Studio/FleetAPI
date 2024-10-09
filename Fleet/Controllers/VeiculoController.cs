using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [ApiController]
    public class VeiculoController(IVeiculoService veiculoService) : ControllerBase
    {
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]")]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromRoute] string WorkspaceId, [FromBody] VeiculoRequest request)
        {
            var id = await veiculoService.Cadastrar(request, WorkspaceId);
            return Ok(new { Id = id });
        }

        [HttpGet("api/Workspace/{WorkspaceId}/[Controller]")]
        [Authorize]
        public async Task<IActionResult> Listar([FromRoute] string WorkspaceId)
        {
            var veiculos = await veiculoService.Listar(WorkspaceId);

            return Ok(veiculos);
        }

        [HttpDelete("api/[Controller]/{VeiculoId}")]
        [Authorize]
        public async Task<IActionResult> Deletar([FromRoute] string VeiculoId)
        {
            await veiculoService.Deletar(VeiculoId);
            return Ok();
        }

        [HttpPut("api/[Controller]/{VeiculoId}")]
        [Authorize]
        public async Task<IActionResult> Atualizar([FromRoute] string VeiculoId, [FromBody] VeiculoPutRequest request)
        {
            await veiculoService.Atualizar(request, VeiculoId);
            return Ok();
        }

        [HttpGet("api/[Controller]/{VeiculoId}")]
        [Authorize]
        public async Task<IActionResult> DataUltimoUso([FromRoute] string VeiculoId)
        {
           var resposta = await veiculoService.BuscarDataUltimoUso(VeiculoId);
            return Ok(resposta);
        }
    }
}