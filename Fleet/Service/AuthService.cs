
using Fleet.Controllers.Model.Request;
using Fleet.Controllers.Model.Request.Auth;
using Fleet.Controllers.Model.Response.Auth;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Enums;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Resources;
using Fleet.Validators;

namespace Fleet.Service;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private string Secret { get => _configuration.GetValue<string>("Crypto:Secret"); }
    private readonly IEmailService _emailService;

    public AuthService(IUsuarioRepository usuarioRepository,
                        ITokenService tokenService,
                        IConfiguration configuration,
                        IEmailService emailService)
    {
        _configuration = configuration;
        _tokenService = tokenService;
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
    }
    public async Task<LoginResponse> Logar(LoginRequest login)
    {
        var usuario =  await _usuarioRepository.Buscar(x => x.Email == login.Email) ?? 
                                throw new UnauthorizedAccessException(Resource.usuario_emailSenhaInvalido);
                                
        if(!(usuario.Email == login.Email) || !(CriptografiaHelper.DescriptografarAes(usuario.Senha, Secret) == login.Senha))
            throw new UnauthorizedAccessException(Resource.usuario_emailSenhaInvalido);
            
        var token =  _tokenService.GenerateToken(usuario);

        var usuarioResponse = new UsuarioResponse(
                                    CriptografiaHelper.CriptografarAes(usuario.Id.ToString(), Secret), 
                                    usuario.Nome, 
                                    usuario.CPF, 
                                    usuario.Email, 
                                    usuario.UrlImagem);
        
        return new LoginResponse(usuarioResponse, token);
    }

    public async Task<EsqueceuSenhaResponse> EsqueceuSenha(EsqueceuSenhaRequest request)
    {
        //busa o usuario se não existir devolve resposta sem codigo e sem token
        var usuario = await _usuarioRepository.Buscar(x => x.Email == request.Email);
        if (usuario == null) return new();

        //se o email do usuario existe gera um codigo e um token
        string codigo = new Random().Next(0, 10000).ToString("D4");
        usuario.Token = Guid.NewGuid().ToString();

        //salva o token na base e envia o email para o usuario
        await _usuarioRepository.Atualizar(usuario.Id, usuario);
        await _emailService.EnviarEmail(usuario.Email, usuario.Nome, "Recuperação de senha", codigo);

        return new EsqueceuSenhaResponse { Codigo = codigo, Token = usuario.Token };
    }

    public async Task AlterarSenha(string token, string senha)
    {
        //se o token nao existir na base devolve erro, se não atualiza a senha do usuario que possue o token
        var usuario = await _usuarioRepository.Buscar(x => x.Token == token) ?? throw new BussinessException("Não foi possivel resetar a senha.");

        usuario.Senha = CriptografiaHelper.CriptografarAes(senha, Secret)!;
        await _usuarioRepository.AtualizarSenha(usuario);
    }

}
