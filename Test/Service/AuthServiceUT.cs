using AutoMapper;
using Bogus;
using Faker;
using Fleet.Controllers.Model.Request;
using Fleet.Controllers.Model.Request.Auth;
using Fleet.Controllers.Model.Response.Auth;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Mapper;
using Fleet.Models;
using Fleet.Service;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;


namespace Test.Service;

public class AuthServiceUT
{
    private Mock<IUsuarioRepository> _usuarioRepository;
    private IAuthService _service;
    private ITokenService _tokenService;
    private IConfiguration _configuration;
    private Mock<IEmailService> _emailService;
    public AuthServiceUT()
    {
        _usuarioRepository = new Mock<IUsuarioRepository>();

        var inMemorySettings = new Dictionary<string, string> {{ "Crypto:Secret", "fikra!123" }, 
                                                                { "Authorization:Secret", "bG9uZzBzZWNyZXQ4Zm9ySmN0U0czNDU2cG9zdA==" },
                                                                { "Credentials:Email_Envio", "contato@fikra.com.br" },
                                                                { "Credentials:Email_Servidor", "mail.fikra.com.br" },
                                                                { "Credentials:Email_Porta", "587" },
                                                                { "Credentials:Email_Senha", "b.9h;w[}7Z=(" }, };

        _configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings)
                            .Build();
        var mappingConfig = new MapperConfiguration( mc => mc.AddProfile(new Mapping()));
        _emailService = new Mock<IEmailService>();
        _tokenService = new TokenService(_configuration);
        _service = new AuthService(_usuarioRepository.Object, _tokenService, _configuration, _emailService.Object);
    }

    [Fact]
    public async Task Retorna_Login()
    {
        var cpf = "103.310.849-96";
        var email = Faker.User.Email();
        var name = Faker.User.Username();
        var password = Faker.User.Password();

        var login = new LoginRequest{
            Email = email,
            Senha = password
        };

        var usuario = new Usuario{
            Id= Faker.Number.RandomNumber(1,int.MaxValue),
            CPF= cpf,
            Email= email,
            Nome= name,
            Senha = CriptografiaHelper.CriptografarAes(password, _configuration.GetValue<string>("Crypto:Secret")) ?? string.Empty
        };

        _usuarioRepository.Setup(x => x.Buscar(It.IsAny<Expression<Func<Usuario,bool>>>()))
                                .ReturnsAsync(usuario);

        LoginResponse response = await _service.Logar(login);                        

        Assert.IsType<LoginResponse>(response);
    }

    [Fact]
    public async Task Esqueceu_Senha_Sucesso()
    {
        var esqueceuSenhaRequest = new EsqueceuSenhaRequest {
            Email = "vitorfidelissantos@gmail.com",
        };
        
        var usuario = new Usuario {
            Nome = "Vitor Hugo",
        };

        _usuarioRepository.Setup(x => x.ExisteEmail(esqueceuSenhaRequest.Email, null))
                                .ReturnsAsync(true);

        _usuarioRepository.Setup(x => x.Buscar(x => x.Email == esqueceuSenhaRequest.Email))
                                .ReturnsAsync(usuario);

        _emailService.Setup(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        var response = await _service.EsqueceuSenha(esqueceuSenhaRequest);
       
        Assert.IsType<EsqueceuSenhaResponse>(response);
        Assert.NotEmpty(response.Codigo);
        Assert.NotEmpty(response.Token);
    }
}
