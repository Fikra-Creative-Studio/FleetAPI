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
        public async Task<IActionResult> Criar(IFormFile? file ,[FromBody] WorkspaceRequest request)
        {
            await worskpaceService.Criar(file ,request);

            return Created();
        }
    }
}
