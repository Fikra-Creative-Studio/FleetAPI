using System;
using Bogus;
using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Fleet.Resources;
using Fleet.Validators;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Moq;

namespace Fleet.Test.Validator;

public class UsuarioValidatorUT
{
    private Mock<IUsuarioRepository> usuarioRepositoryMock;



    public UsuarioValidatorUT()
    {
        usuarioRepositoryMock = new Mock<IUsuarioRepository>();
    }

    [Fact]
    public async Task Validar_CPF_Invalido()
    {
        var validator = new UsuarioValidator(usuarioRepositoryMock.Object, UsuarioRequestEnum.Criar);
        var usuario = new Usuario
        {
            Email = "teste@gmail.com",
            Nome=Faker.User.Username(),
            Ativo = true,
            Id = Faker.Number.RandomNumber(1, int.MaxValue),
            CPF = "000.000.000-00",
            Senha = "Senha123!"
        };
        //Act
        var validadorUsuario = await validator.ValidateAsync(usuario);

        //Assert
        validadorUsuario.IsValid.Should().BeFalse();
        validadorUsuario.Errors.Should().Contain(x => x.ErrorMessage == "CPF invalido");

    }

    [Fact]
    public async Task UsuarioValidator_Validar_EmailDuplicado()
    {
        var validator = new UsuarioValidator(usuarioRepositoryMock.Object, UsuarioRequestEnum.Criar);
        var email = "teste@gmail.com";
        //Arrange
        var usuario = new Usuario
        {
            Email = email,
            Ativo = true,
            Nome=Faker.User.Username(),
            Id = Faker.Number.RandomNumber(1, int.MaxValue),
            CPF = "054.214.046-25",
            Senha = "Senha123!"
        };
        usuarioRepositoryMock.Setup( x => x.ExisteEmail(usuario.Email, null))
                                .ReturnsAsync(true);

        //Act
        var validadorUsuario = await validator.ValidateAsync(usuario);

        //Assert
        validadorUsuario.IsValid.Should().BeFalse();
        validadorUsuario.Errors.Should().Contain(x => x.ErrorMessage == Resource.usuario_emailduplicado);
    }

    [Fact]
    public async Task UsuarioValidator_Validar_EmailIncorreto()
    {
        var validator = new UsuarioValidator(usuarioRepositoryMock.Object, UsuarioRequestEnum.Criar);
        var usuario = new Usuario
        {
            Email = "testemail.com",
            Ativo = true,
            Nome=Faker.User.Username(),
            Id = Faker.Number.RandomNumber(1, int.MaxValue),
            CPF = "054.214.046-25",
            Senha = "Senha123!"
        };

        //Act
        var validadorUsuario = await validator.ValidateAsync(usuario);

        //Assert
        //Assert.NotEmpty(validationResult.Errors); -> padrão do xunit
        validadorUsuario.Errors.Should().NotBeNullOrEmpty();
        validadorUsuario.Errors.Should().Contain(x => x.ErrorMessage == Resource.usario_emailInvalido);
    }

    [Fact]
    public async void UsuarioValidator_Validar_EmailVazio()
    {
        var validator = new UsuarioValidator(usuarioRepositoryMock.Object, UsuarioRequestEnum.Criar);
        //Arrange
        var usuario = new Usuario
        {
            Email = string.Empty,
            Ativo = true,
            Nome=Faker.User.Username(),
            Id = Faker.Number.RandomNumber(1, int.MaxValue),
            CPF = "054.214.046-25",
            Senha = "Senha123!"
        };

        //Act
        var validadorUsuario = await validator.ValidateAsync(usuario);

        //Assert
        //Assert.NotEmpty(validationResult.Errors); -> padrão do xunit
        validadorUsuario.Errors.Should().NotBeNullOrEmpty();
        validadorUsuario.Errors.Should().Contain(x => x.ErrorMessage == Resource.usario_emailInvalido);
    }

}
