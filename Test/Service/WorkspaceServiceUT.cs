using System.Linq.Expressions;
using AutoMapper;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Fleet.Enums;

namespace Test.Service;

public class WorkspaceServiceUT
{
    private Mock<IUsuarioRepository> _usuarioRepository;
    private Mock<IUsuarioWorkspaceRepository> _usuarioWorkspaceRepository;
    private Mock<IWorkspaceRepository> _workspaceRepository;
    private Mock<ILoggedUser> _loggedUser;
    private Mock<IMapper> _mapper;
    private Mock<IBucketService> _bucketService;
    private IConfiguration _configuration;
    IWorskpaceService _worskpaceService;
    private Mock<IEmailService> _emailService;
    public WorkspaceServiceUT()
    {
        _usuarioRepository = new Mock<IUsuarioRepository>();
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _usuarioWorkspaceRepository = new Mock<IUsuarioWorkspaceRepository>();
        _bucketService = new Mock<IBucketService>();
        _mapper = new Mock<IMapper>();
        _loggedUser = new Mock<ILoggedUser>();
        _emailService = new Mock<IEmailService>();
        var inMemorySettings = new Dictionary<string, string> {{ "Crypto:Secret", "fleet123!@#" } };

        _configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings)
                            .Build();
        _worskpaceService = new WorkspaceService(_loggedUser.Object,
                                                _workspaceRepository.Object,
                                                _usuarioWorkspaceRepository.Object,
                                                _usuarioRepository.Object,
                                                _mapper.Object,
                                                _bucketService.Object, 
                                                _configuration,
                                                _emailService.Object);
        _loggedUser.Setup(x => x.UserId).Returns(Faker.Number.RandomNumber());
    }

    [Fact]
    public async Task Criar_Workspace_Sucesso_Sem_Imagem()
    {
        var cnpj = "35.473.394/0001-80";
        var fantasia = "Teste";

        WorkspaceRequest request = new () {
            Fantasia = fantasia,
            CNPJ = cnpj
        };

        Usuario usuario = new() {
                Id= Faker.Number.RandomNumber(1,int.MaxValue),
                CPF= "103.310.849-96",
                Email= Faker.User.Email(),
                Nome= Faker.User.Username(),
                Senha = Faker.User.Password()
        };

        Workspace workspace = new() {
            Id= Faker.Number.RandomNumber(1,int.MaxValue),
            Fantasia = fantasia,
            Cnpj = cnpj
        };

        _mapper.Setup(mapper => mapper.Map<Workspace>(request))
            .Returns(workspace);

        _usuarioRepository.Setup(x => x.Buscar(It.IsAny<Expression<Func<Usuario, bool>>>()))
                            .ReturnsAsync(usuario);
        
        _workspaceRepository.Setup(x => x.Criar(workspace)).ReturnsAsync(workspace);

        await _worskpaceService.Criar(null,request);
        _usuarioRepository.Verify(repo => repo.Buscar(It.IsAny<Expression<Func<Usuario, bool>>>()), Times.Once);
        _mapper.Verify(mapper => mapper.Map<Workspace>(request), Times.Once);
        _workspaceRepository.Verify(repo => repo.Criar(workspace), Times.Once);
        _usuarioWorkspaceRepository.Verify(repo => repo.Criar(It.IsAny<UsuarioWorkspace>()), Times.Once);
    }

    [Fact]
    public async Task Criar_Workspace_Sucesso_Com_Imagem()
    {
        var cnpj = "35.473.394/0001-80";
        var fantasia = "Teste";

        WorkspaceRequest request = new () {
            Fantasia = fantasia,
            CNPJ = cnpj
        };

        Usuario usuario = new() {
                Id= Faker.Number.RandomNumber(1,int.MaxValue),
                CPF= "103.310.849-96",
                Email= Faker.User.Email(),
                Nome= Faker.User.Username(),
                Senha = Faker.User.Password()
        };

        Workspace workspace = new() {
            Id= Faker.Number.RandomNumber(1,int.MaxValue),
            Fantasia = fantasia,
            Cnpj = cnpj
        };

        var fileMock = new Mock<IFormFile>();
        var content = "Test content";
        var fileName = "test.png";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        var extension = "png";
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);

        _mapper.Setup(mapper => mapper.Map<Workspace>(request))
            .Returns(workspace);

        _usuarioRepository.Setup(x => x.Buscar(It.IsAny<Expression<Func<Usuario, bool>>>()))
                            .ReturnsAsync(usuario);
        _bucketService.Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>())).ReturnsAsync("http://test.teste");
        
        _workspaceRepository.Setup(x => x.Criar(It.IsAny<Workspace>())).ReturnsAsync(workspace);

        await _worskpaceService.Criar(fileMock.Object,request);

        _usuarioRepository.Verify(x => x.Buscar(It.IsAny<Expression<Func<Usuario, bool>>>()), Times.Once);
        _bucketService.Verify(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
        _workspaceRepository.Verify(x => x.Criar(It.IsAny<Workspace>()), Times.Once);
        _usuarioWorkspaceRepository.Verify(x => x.Criar(It.IsAny<UsuarioWorkspace>()), Times.Once);
    }

    [Fact]
    public async Task Buscar_Usuarios_Sucesso()
    {
        var id = Faker.Number.RandomNumber(1,int.MaxValue);
        var encryptId = CriptografiaHelper.CriptografarAes(id.ToString(), "fleet123!@#");
        
        var usuarios = new List<Usuario>
        {
            new() {
                Id= Faker.Number.RandomNumber(1,int.MaxValue),
                CPF= "111.111.111-02",
                Email= Faker.User.Email(),
                Nome= Faker.User.Username(),
                Senha = CriptografiaHelper.CriptografarAes(Faker.User.Password(), _configuration.GetValue<string>("Crypto:Secret")) ?? string.Empty
            }
        }.AsQueryable();

        _usuarioWorkspaceRepository.SetupSequence(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true);
        
        _usuarioRepository.Setup(x => x.Listar(It.IsAny<Expression<Func<Usuario, bool>>>()))
                                .Returns((Expression<Func<Usuario, bool>> exp) => usuarios);

        var response = await _worskpaceService.BuscarUsuarios(encryptId);

        Assert.IsType<List<UsuarioBuscarWorkspaceResponse>>(response);
    }

    [Fact]
    public async Task Atualizar_Papel_Sucesso()
    {
        var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);
        WorkspaceAtualizarPermissaoRequest request = new(){
            UsuarioId =  CriptografiaHelper.CriptografarAes(usuarioId.ToString(), "fleet123!@#"),
            Papel = PapelEnum.Administrador
        };

        Usuario usuario = new() {
            Id= Faker.Number.RandomNumber(1,int.MaxValue),
            CPF= "111.111.111-02",
            Email= Faker.User.Email(),
            Nome= Faker.User.Username(),
            Senha = CriptografiaHelper.CriptografarAes(Faker.User.Password(), _configuration.GetValue<string>("Crypto:Secret")) ?? string.Empty
        };

        Workspace workspace = new() {
            Id= workspaceId,
            Fantasia = "Teste"
        };

        _usuarioWorkspaceRepository.SetupSequence(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true);
        
        await _worskpaceService.AtualizarPapel(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), request);

        _usuarioWorkspaceRepository.Verify(x => x.AtualizarPapel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<PapelEnum>()), Times.Once);
    }

    [Fact]
    public async Task Convidar_Usuario_Existente_Sucesso()
    {
        var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);

        Usuario usuario = new() {
            Id= Faker.Number.RandomNumber(1,int.MaxValue),
            CPF= "111.111.111-02",
            Email= Faker.User.Email(),
            Nome= Faker.User.Username(),
            Senha = CriptografiaHelper.CriptografarAes(Faker.User.Password(), _configuration.GetValue<string>("Crypto:Secret")) ?? string.Empty
        };

        Workspace workspace = new() {
            Id= workspaceId,
            Fantasia = "Teste"
        };

         _usuarioWorkspaceRepository.Setup(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true);
        
        _usuarioRepository.Setup(x => x.Buscar(It.IsAny<Expression<Func<Usuario, bool>>>()))
                            .ReturnsAsync(usuario);
        
        _workspaceRepository.Setup(x => x.Buscar(It.IsAny<Expression<Func<Workspace, bool>>>()))
                                .ReturnsAsync(workspace);
        
        await _worskpaceService.ConvidarUsuario(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), usuario.Email);

        _usuarioWorkspaceRepository.Verify(x => x.Criar(It.IsAny<UsuarioWorkspace>()), Times.Once);
        _emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Convidar_Usuario_Inexistente_Sucesso()
    {
        var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);

        Workspace workspace = new() {
            Id= workspaceId,
            Fantasia = "Teste"
        };

        _usuarioWorkspaceRepository.Setup(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true);
        
        
        _workspaceRepository.Setup(x => x.Buscar(It.IsAny<Expression<Func<Workspace, bool>>>()))
                                .ReturnsAsync(workspace);
        
        await _worskpaceService.ConvidarUsuario(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), "teste@teste.com.br");

        _usuarioWorkspaceRepository.Verify(x => x.Criar(It.IsAny<UsuarioWorkspace>()), Times.Once);
        _emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Remover_Usuario_Sucesso_Usuario_Admin()
    {
        var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);

        _usuarioWorkspaceRepository.SetupSequence(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true);

        await _worskpaceService.RemoverUsuario(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), CriptografiaHelper.CriptografarAes(usuarioId.ToString(), "fleet123!@#"));

        _usuarioWorkspaceRepository.Verify(x => x.Remover(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

    }

    [Fact]
    public async Task Remover_Usuario_Sucesso_Usuario_Nao_Admin()
    {
        var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);

        _usuarioWorkspaceRepository.SetupSequence(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(false)
                                    .ReturnsAsync(true);

        await _worskpaceService.RemoverUsuario(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), CriptografiaHelper.CriptografarAes(usuarioId.ToString(), "fleet123!@#"));

        _usuarioWorkspaceRepository.Verify(x => x.Remover(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Remover_Usuario_Erro_Usuario_Unico_Admin()
    {
          var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);

        _usuarioWorkspaceRepository.SetupSequence(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<BussinessException>(async () =>
        {
             await _worskpaceService.RemoverUsuario(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), CriptografiaHelper.CriptografarAes(usuarioId.ToString(), "fleet123!@#"));
        });

        Assert.Equal("Usuario não pode ser removido pois é o único administrador desse workspace", exception.Message);
    }

    [Fact]
    public async Task Remover_Usuario_Erro_Usuario_Nao_Existe_No_Workspace()
    {
          var usuarioId = Faker.Number.RandomNumber(1,int.MaxValue);
        var workspaceId = Faker.Number.RandomNumber(1,int.MaxValue);

        _usuarioWorkspaceRepository.SetupSequence(x => x.Existe(It.IsAny<Expression<Func<UsuarioWorkspace, bool>>>()))
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(false)
                                    .ReturnsAsync(true)
                                    .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<BussinessException>(async () =>
        {
             await _worskpaceService.RemoverUsuario(CriptografiaHelper.CriptografarAes(workspaceId.ToString(), "fleet123!@#"), CriptografiaHelper.CriptografarAes(usuarioId.ToString(), "fleet123!@#"));
        });

        Assert.Equal("Usuário não encontrado nesse workspace", exception.Message);
    }
}