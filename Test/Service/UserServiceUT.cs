using AutoMapper;
using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Mapper;
using Fleet.Models;
using Fleet.Service;
using Microsoft.Extensions.Configuration;
using Moq;


namespace Fleet.Test.Service;

public class UserServiceUT
{
    private Mock<IUsuarioRepository> _usuarioRepository;
    private Mock<IBucketService> _bucketService;
    private IUsuarioService _service;
    private IConfiguration _configuration;
    private IMapper _mapper;
    public UserServiceUT()
    {
        _usuarioRepository = new Mock<IUsuarioRepository>();

         var inMemorySettings = new Dictionary<string, string> {{ "Crypto:Secret", "fleet123!@#" } };

        _configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings)
                            .Build();
        var mappingConfig = new MapperConfiguration( mc => mc.AddProfile(new Mapping()));
        _mapper = mappingConfig.CreateMapper();


        _bucketService = new Mock<IBucketService>();
        _service = new UsuarioService(_usuarioRepository.Object, _configuration, _mapper, _bucketService.Object);
    }

    [Fact]
    public async Task Criar_Usuario_Sucesso()
    {
        var cpf = "054.214.046-25";
        var email = "teste@email.com";
        var name = Faker.User.Username();
        var password = Faker.User.Password();
        var criptoPassword = CriptografiaHelper.CriptografarAes(password, "fleet123!@#");
        var usuarioRequest = new UsuarioRequest{
            CPF= cpf,
            Email= email,
            Nome= name,
            Senha = password
        };

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
        var usuarioRequest = new UsuarioRequest{
            CPF= cpf,
            Email= email,
            Nome= name,
            Senha = password
        };
        Assert.ThrowsAsync<BussinessException>(async() => await _service.Criar(usuarioRequest));
    }
}