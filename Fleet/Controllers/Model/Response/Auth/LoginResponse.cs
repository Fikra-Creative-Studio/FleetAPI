using System;
using Fleet.Controllers.Model.Response.Usuario;

namespace Fleet.Controllers.Model.Response.Auth;

public class LoginResponse(UsuarioResponse usuario , string token)
{
    public UsuarioResponse usuario { get; set; } = usuario;
    public string token { get; set; }  = token;

}
