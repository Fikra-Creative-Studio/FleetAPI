
using Fleet.Controllers.Model.Request;
using Fleet.Controllers.Model.Request.Auth;
using Fleet.Controllers.Model.Response.Auth;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Controllers.Model.Response.Workspace;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Resources;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
        var usuario = _usuarioRepository.Listar(x => x.Ativo && x.Email == login.Email && x.Senha == CriptografiaHelper.CriptografarAes(login.Senha, Secret))
                                .Include(x => x.UsuarioWorkspaces.Where(uw => uw.Ativo && uw.Workspace.Ativo))
                                .ThenInclude(uw => uw.Workspace)
                                .FirstOrDefault() ??  throw new UnauthorizedAccessException(Resource.usuario_emailSenhaInvalido);
                                
        var token =  _tokenService.GenerateToken(usuario);

        var usuarioResponse = new UsuarioResponse
        {
            Id = CriptografiaHelper.CriptografarAes(usuario.Id.ToString(), Secret),
            Nome = usuario.Nome,
            CPF = usuario.CPF,
            Email = usuario.Email,
            UrlImagem = usuario.UrlImagem,
            Workspaces = usuario.UsuarioWorkspaces.Select(x => new WorkspaceGetResponse { 
                Id = CriptografiaHelper.CriptografarAes(x.Workspace.Id.ToString(), Secret),
                Cnpj = x.Workspace.Cnpj,
                Fantasia = x.Workspace.Fantasia,
                UrlImage = x.Workspace.UrlImagem,
                Papel = x.Papel
            }).ToList(),
        };
        
        return new LoginResponse(usuarioResponse, token);
    }

    public async Task<EsqueceuSenhaResponse> EsqueceuSenha(EsqueceuSenhaRequest request)
    {
        //busa o usuario se não existir devolve resposta sem codigo e sem token
        var usuario = await _usuarioRepository.Buscar(x => x.Email == request.Email && x.Ativo);
        if (usuario == null) return new();

        //se o email do usuario existe gera um codigo e um token
        string codigo = new Random().Next(0, 100000).ToString("D5");
        usuario.Token = Guid.NewGuid().ToString();

        //salva o token na base e envia o email para o usuario
        await _usuarioRepository.Atualizar(usuario.Id, usuario);

        string codigoTemplate = string.Empty;
        codigo.ToList().ForEach(x => { codigoTemplate += $"<td>{x}</td>"; });

        string mailPath = $"{AppDomain.CurrentDomain.BaseDirectory}Service\\TemplateMail\\forgot-password.html";
        string fileContent = await File.ReadAllTextAsync(mailPath, Encoding.UTF8);
        fileContent = fileContent.Replace("{{senha}}", codigoTemplate);
        await _emailService.EnviarEmail(usuario.Email, usuario.Nome, "Recuperação de senha", fileContent);

        return new EsqueceuSenhaResponse { Codigo = codigo, Token = usuario.Token };
    }

    public async Task AlterarSenha(string token, string senha)
    {
        //se o token nao existir na base devolve erro, se não atualiza a senha do usuario que possue o token
        var usuario = await _usuarioRepository.Buscar(x => x.Token == token) ?? throw new BussinessException("Não foi possivel resetar a senha.");

        usuario.Senha = CriptografiaHelper.CriptografarAes(senha, Secret)!;
        await _usuarioRepository.AtualizarSenha(usuario);

        string mailPath = $"{AppDomain.CurrentDomain.BaseDirectory}Service\\TemplateMail\\reset-password.html";
        string fileContent = await File.ReadAllTextAsync(mailPath, Encoding.UTF8);
        fileContent = fileContent.Replace("{{name}}", usuario.Nome);
        await _emailService.EnviarEmail(usuario.Email, usuario.Nome, "Sua senha foi alterada", fileContent);
    }

}
