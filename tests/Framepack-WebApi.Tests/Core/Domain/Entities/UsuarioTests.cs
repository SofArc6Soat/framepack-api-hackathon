using Domain.Entities;
using FluentValidation.TestHelper;

namespace Framepack_WebApi.Tests.Core.Domain.Entities;

public class UsuarioTests
{
    [Fact]
    public void Usuario_DeveSerCriadoComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Nome Teste";
        var email = "email@teste.com";

        // Act
        var usuario = new Usuario(id, nome, email);

        // Assert
        Assert.Equal(id, usuario.Id);
        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(email, usuario.Email);
    }

    [Fact]
    public void ValidarUsuario_DeveValidarIdComSucesso()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), "Nome Teste", "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldNotHaveValidationErrorFor(u => u.Id);
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoIdForNulo()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.Empty, "Nome Teste", "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Id)
            .WithErrorMessage("O Id deve ser válido.");
    }

    [Fact]
    public void ValidarUsuario_DeveValidarNomeComSucesso()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), "Nome Teste", "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldNotHaveValidationErrorFor(u => u.Nome);
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoNomeForNulo()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), null, "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Nome)
            .WithErrorMessage("O Nome não pode ser nulo.");
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoNomeForMuitoCurto()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), "A", "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Nome)
            .WithErrorMessage("O Nome precisa ter entre 2 e 50 caracteres e foi informado A.");
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoNomeForMuitoLongo()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var nomeLongo = new string('A', 51);
        var usuario = new Usuario(Guid.NewGuid(), nomeLongo, "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Nome)
            .WithErrorMessage($"O Nome precisa ter entre 2 e 50 caracteres e foi informado {nomeLongo}.");
    }

    [Fact]
    public void ValidarUsuario_DeveValidarEmailComSucesso()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), "Nome Teste", "email@teste.com");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldNotHaveValidationErrorFor(u => u.Email);
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoEmailForNulo()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), "Nome Teste", null);

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Email)
            .WithErrorMessage("O Email não pode ser nulo.");
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoEmailForInvalido()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var usuario = new Usuario(Guid.NewGuid(), "Nome Teste", "email_invalido");

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Email)
            .WithErrorMessage("O Email está em um formato inválido.");
    }

    [Fact]
    public void ValidarUsuario_DeveRetornarErroQuandoEmailForMuitoLongo()
    {
        // Arrange
        var validator = new ValidarUsuario();
        var emailLongo = new string('a', 101) + "@teste.com";
        var usuario = new Usuario(Guid.NewGuid(), "Nome Teste", emailLongo);

        // Act & Assert
        var result = validator.TestValidate(usuario);
        result.ShouldHaveValidationErrorFor(u => u.Email)
            .WithErrorMessage($"O Email precisa ter entre 2 e 100 caracteres e foi informado {emailLongo}.");
    }
}