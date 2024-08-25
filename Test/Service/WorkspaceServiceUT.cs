using System;
using System.Linq.Expressions;
using AutoMapper;
using Castle.Core.Configuration;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Mapper;
using Fleet.Models;
using Fleet.Service;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Test.Service;

public class WorkspaceServiceUT
{
    private Mock<IUsuarioRepository> _usuarioRepository;
    private Mock<IUsuarioWorkspaceRepository> _usuarioWorkspaceRepository;
    private Mock<IWorkspaceRepository> _workspaceRepository;
    private Mock<ILoggedUser> _loggedUser;
    private Mock<IMapper> _mapper;
    private Mock<IBucketService> _bucketService;
    IWorskpaceService _worskpaceService;
    public WorkspaceServiceUT()
    {
        _usuarioRepository = new Mock<IUsuarioRepository>();
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _usuarioWorkspaceRepository = new Mock<IUsuarioWorkspaceRepository>();
        _bucketService = new Mock<IBucketService>();
        _mapper = new Mock<IMapper>();
        _loggedUser = new Mock<ILoggedUser>();
        _worskpaceService = new WorkspaceService(_loggedUser.Object,
                                                _workspaceRepository.Object,
                                                _usuarioWorkspaceRepository.Object,
                                                _usuarioRepository.Object,
                                                _mapper.Object,
                                                _bucketService.Object);
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
}
