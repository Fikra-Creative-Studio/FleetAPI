using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Filters;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController(IWorskpaceService worskpaceService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [ServiceFilter(typeof(TokenFilter))]
        public async Task<IActionResult> Criar([FromBody] WorkspaceRequest request)
        {
            var id = HttpContext.Items["user"] as string;
            
            await worskpaceService.Criar(id, request);

            return Created();
        }
    }
}
