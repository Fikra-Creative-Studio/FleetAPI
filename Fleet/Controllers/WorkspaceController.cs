using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController(IConfiguration configuration, IWorskpaceService worskpaceService) : ControllerBase
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Criar([FromBody] WorkspaceRequest request)
        {
            var workspace = await worskpaceService.Criar(request);
            return Ok(new { Id = CriptografiaHelper.CriptografarAes(workspace.Id.ToString(), Secret) });
        }

        [HttpGet("{WorkspaceId}/Usuarios")]
        [Authorize]
        public async Task<IActionResult> BuscarUsuario([FromRoute] string WorkspaceId)
        {
            var usuarios = await worskpaceService.BuscarUsuarios(WorkspaceId);

            return Ok(usuarios);
        }

        [HttpPatch("{WorkspaceId}/Permissao")]
        [Authorize]
        public async Task<IActionResult> AtualizarPermissao([FromRoute] string WorkspaceId ,[FromBody] WorkspaceAtualizarPermissaoRequest request)
        {
            await worskpaceService.AtualizarPapel(WorkspaceId ,request);

            return Ok($"Permissão do usuário alterada para {request.Papel}");
        }

        [HttpPost("{WorkspaceId}/Convidar")]
        [Authorize]
        public async Task<IActionResult> ConvidarUsuario([FromRoute] string WorkspaceId ,[FromBody] WorkspaceConvidarRequest request)
        {
            var id = await worskpaceService.ConvidarUsuario(WorkspaceId ,request.Email);

            return Ok(new { Id = id });
        }

        [HttpDelete("{WorkspaceId}/Remover/{UsuarioId}")]
        [Authorize]
        public async Task<IActionResult> RemoverUsuario([FromRoute] string WorkspaceId , string UsuarioId)
        {
            await worskpaceService.RemoverUsuario(WorkspaceId , UsuarioId);

            return Ok();
        }

        [HttpPost("{WorkspaceId}/ReenviarConvite/{UsuarioId}")]
        [Authorize]
        public async Task<IActionResult> ReenviarConviteUsuario([FromRoute] string WorkspaceId , string UsuarioId)
        {
            await worskpaceService.ReenviarConviteUsuario(WorkspaceId , UsuarioId);

            return Ok();
        }
    }
}
