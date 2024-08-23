using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fleet.Interfaces.Service;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Service;

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
            var clain = User.Claims.FirstOrDefault(x => x.Type == "user");
            if (clain == null ||  string.IsNullOrEmpty(clain.Value))
                return Unauthorized();

            await usuarioService.Atualizar(clain.Value, usuarioRequest);

            return Ok();
        }


        [HttpPost("[Action]")]
        [Authorize]
        public async Task<IActionResult> Imagem(IFormFile file)
        {
            var clain = User.Claims.FirstOrDefault(x => x.Type == "user");
            if (clain == null || string.IsNullOrEmpty(clain.Value))
                return Unauthorized();

            if (file.Length > 0)
            {
                var extension = file.FileName.Split(".").Last();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    await usuarioService.UploadAsync(clain.Value, stream, extension);
                }
                return Ok("Arquivo enviado com sucesso!");
            }
            return BadRequest("Arquivo inválido.");
        }
    }
}