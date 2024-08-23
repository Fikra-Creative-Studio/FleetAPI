using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fleet.Interfaces.Service;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Service;
using Fleet.Filters;

namespace Fleet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
    {

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Criar([FromBody] UsuarioRequest usuarioRequest)
        {
            await usuarioService.Criar(usuarioRequest);

            return Created();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Atualizar([FromBody] UsurioPutRequest usuarioRequest)
        {
            await usuarioService.Atualizar(usuarioRequest);

            return Ok();
        }


        [HttpPost("[Action]")]
        [Authorize]
        public async Task<IActionResult> Imagem(IFormFile file)
        {
            var id = HttpContext.Items["user"] as string;

            if (file.Length > 0)
            {
                var extension = file.FileName.Split(".").Last();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    await usuarioService.UploadAsync(id, stream, extension);
                }
                return Ok("Arquivo enviado com sucesso!");
            }
            return BadRequest("Arquivo inválido.");
        }
    }
}