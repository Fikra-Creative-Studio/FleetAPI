using AutoMapper;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Mapper;
using Fleet.Models;
using Fleet.Repository;
using Fleet.Service;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Reflection;


namespace Test.Service;

public class UserServiceUT
{
    private Mock<IUsuarioRepository> _usuarioRepository;
    private Mock<IBucketService> _bucketService;
    private Mock<ILoggedUser> _loggedUser;
    private Mock<IEmailService> _emailService;
    private IUsuarioService _service;
    private IConfiguration _configuration;
    private IMapper _mapper;
    public UserServiceUT()
    {
        _usuarioRepository = new Mock<IUsuarioRepository>();

        var inMemorySettings = new Dictionary<string, string> { { "Crypto:Secret", "fleet123!@#" } };

        _configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings)
                            .Build();
        var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new Mapping()));
        _mapper = mappingConfig.CreateMapper();


        _bucketService = new Mock<IBucketService>();
        _loggedUser = new Mock<ILoggedUser>();
        _emailService = new Mock<IEmailService>();

        _service = new UsuarioService(_usuarioRepository.Object, _configuration, _mapper, _bucketService.Object, _loggedUser.Object, _emailService.Object);

        _loggedUser.Setup(x => x.UserId).Returns(Faker.Number.RandomNumber());
    }

    [Fact]
    public async Task Criar_Usuario_Sucesso()
    {
        var cpf = "054.214.046-25";
        var email = "teste@email.com";
        var name = Faker.User.Username();
        var password = Faker.User.Password();
        var criptoPassword = CriptografiaHelper.CriptografarAes(password, "fleet123!@#");
        var usuarioRequest = new UsuarioRequest
        {
            CPF = cpf,
            Email = email,
            Nome = name,
            Senha = password
        };

        _usuarioRepository.Setup(x => x.Criar(It.IsAny<Usuario>())).Returns(Task.Run(() => new Usuario()));
        _emailService.Setup(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        await _service.Criar(usuarioRequest);

        _usuarioRepository.Verify(x => x.Criar(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task Criar_Usuario_Erro_CPF()
    {
        var cpf = "111.111.111-02";
        var email = Faker.User.Email();
        var name = Faker.User.Username();
        var password = Faker.User.Password();
        var criptoPassword = CriptografiaHelper.CriptografarAes(password, "fleet123!@#");
        var usuarioRequest = new UsuarioRequest
        {
            CPF = cpf,
            Email = email,
            Nome = name,
            Senha = password
        };
        Assert.ThrowsAsync<BussinessException>(async () => await _service.Criar(usuarioRequest));
    }
}