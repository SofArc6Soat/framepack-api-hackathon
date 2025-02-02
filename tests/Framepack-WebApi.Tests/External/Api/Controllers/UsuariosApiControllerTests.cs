using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Framepack_WebApi.Tests.External.Api.Controllers;

public class UsuariosApiControllerTests
{
    private readonly Mock<IUsuarioController> _usuarioControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly UsuariosApiController _controller;

    public UsuariosApiControllerTests()
    {
        _usuarioControllerMock = new Mock<IUsuarioController>();
        _notificadorMock = new Mock<INotificador>();
        _controller = new UsuariosApiController(_usuarioControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task CadastrarUsuarioAsync_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "ModelState is invalid");

        // Act
        var result = await _controller.CadastrarUsuarioAsync(new UsuarioRequestDto { Id = Guid.NewGuid(), Nome = "Test", Email = "test@example.com", Senha = "password123" }, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CadastrarUsuarioAsync_Success_ReturnsCreatedResult()
    {
        // Arrange
        var request = new UsuarioRequestDto { Id = Guid.NewGuid(), Nome = "Test", Email = "test@example.com", Senha = "password123" };
        _usuarioControllerMock.Setup(x => x.CadastrarUsuarioAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.CadastrarUsuarioAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task IdentificarUsuario_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "ModelState is invalid");

        // Act
        var result = await _controller.IdentificarUsuario(new IdentifiqueSeRequestDto { Email = "test@example.com", Senha = "password123" }, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task IdentificarUsuario_Success_ReturnsCreatedResult()
    {
        // Arrange
        var request = new IdentifiqueSeRequestDto { Email = "test@example.com", Senha = "password123" };
        var tokenUsuario = new TokenUsuario { Email = "test@example.com", AccessToken = "accessToken", RefreshToken = "refreshToken", Expiry = DateTimeOffset.UtcNow };
        _usuarioControllerMock.Setup(x => x.IdentificarUsuarioAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(tokenUsuario);

        // Act
        var result = await _controller.IdentificarUsuario(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task ConfirmarEmailVerificaoAsync_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "ModelState is invalid");

        // Act
        var result = await _controller.ConfirmarEmailVerificaoAsync(new ConfirmarEmailVerificacaoDto { Email = "test@example.com", CodigoVerificacao = "123456" }, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificaoAsync_Success_ReturnsCreatedResult()
    {
        // Arrange
        var request = new ConfirmarEmailVerificacaoDto { Email = "test@example.com", CodigoVerificacao = "123456" };
        _usuarioControllerMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.ConfirmarEmailVerificaoAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "ModelState is invalid");

        // Act
        var result = await _controller.SolicitarRecuperacaoSenhaAsync(new SolicitarRecuperacaoSenhaDto { Email = "test@example.com" }, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Success_ReturnsCreatedResult()
    {
        // Arrange
        var request = new SolicitarRecuperacaoSenhaDto { Email = "test@example.com" };
        _usuarioControllerMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.SolicitarRecuperacaoSenhaAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "ModelState is invalid");

        // Act
        var result = await _controller.EfetuarResetSenhaAsync(new ResetarSenhaDto { Email = "test@example.com", CodigoVerificacao = "123456", NovaSenha = "newpassword123" }, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Success_ReturnsCreatedResult()
    {
        // Arrange
        var request = new ResetarSenhaDto { Email = "test@example.com", CodigoVerificacao = "123456", NovaSenha = "newpassword123" };
        _usuarioControllerMock.Setup(x => x.EfetuarResetSenhaAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.EfetuarResetSenhaAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }
}
