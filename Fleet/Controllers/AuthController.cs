using Fleet.Controllers.Model.Request;
using Fleet.Controllers.Model.Request.Auth;
using Fleet.Controllers.Model.Response.Auth;
using Fleet.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> Logar([FromBody] LoginRequest loginRequest)
        {
            LoginResponse response = await _authService.Logar(loginRequest);
            return Ok(response);
        }

        /// <summary>
        /// Recebe e-mail e código para resetar a senha
        /// </summary>
        [HttpPost("[Action]")]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueceuSenha([FromBody] EsqueceuSenhaRequest request) 
        {
            var response = await _authService.EsqueceuSenha(request);
            return Ok(response);
        }


        [HttpPost("[Action]/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetarSenha([FromBody] AtualizarSenhaRequest request, string token) 
        {
            await _authService.AlterarSenha(token,request.Senha);
            return Ok();
        }
    }
}
