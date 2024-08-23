using Fleet.Controllers.Model.Request;
using Fleet.Controllers.Model.Request.Auth;
using Fleet.Controllers.Model.Response.Auth;

namespace Fleet.Interfaces.Service;

public interface IAuthService
{
    Task<LoginResponse> Logar(LoginRequest login);
    Task<EsqueceuSenhaResponse> EsqueceuSenha(EsqueceuSenhaRequest request);
    Task AlterarSenha(string token, string senha);
}
