using Controllers;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Adpters.Controllers;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioUseCase> _usuarioUseCaseMock;
    private readonly UsuarioController _usuarioController;

    public UsuarioControllerTests()
    {
        _usuarioUseCaseMock = new Mock<IUsuarioUseCase>();
        _usuarioController = new UsuarioController(_usuarioUseCaseMock.Object);
    }

    [Fact]
    public async Task CadastrarUsuarioAsync_Success()
    {
        // Arrange
        var usuarioRequestDto = new UsuarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Teste",
            Email = "teste@teste.com",
            Senha = "Senha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.CadastrarUsuarioAsync(It.IsAny<Usuario>(), usuarioRequestDto.Senha, cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken);

        // Assert
        Assert.True(result);
        _usuarioUseCaseMock.Verify(x => x.CadastrarUsuarioAsync(It.IsAny<Usuario>(), usuarioRequestDto.Senha, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CadastrarUsuarioAsync_Failure()
    {
        // Arrange
        var usuarioRequestDto = new UsuarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Teste",
            Email = "teste@teste.com",
            Senha = "Senha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.CadastrarUsuarioAsync(It.IsAny<Usuario>(), usuarioRequestDto.Senha, cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioController.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken);

        // Assert
        Assert.False(result);
        _usuarioUseCaseMock.Verify(x => x.CadastrarUsuarioAsync(It.IsAny<Usuario>(), usuarioRequestDto.Senha, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task IdentificarUsuarioAsync_Success()
    {
        // Arrange
        var identifiqueSeRequestDto = new IdentifiqueSeRequestDto
        {
            Email = "teste@teste.com",
            Senha = "Senha123!"
        };
        var tokenUsuario = new TokenUsuario
        {
            Email = "teste@teste.com",
            AccessToken = "access_token",
            RefreshToken = "refresh_token",
            Expiry = DateTimeOffset.UtcNow.AddHours(1)
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto.Email, identifiqueSeRequestDto.Senha, cancellationToken))
            .ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuarioController.IdentificarUsuarioAsync(identifiqueSeRequestDto, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tokenUsuario.Email, result?.Email);
        _usuarioUseCaseMock.Verify(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto.Email, identifiqueSeRequestDto.Senha, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task IdentificarUsuarioAsync_Failure()
    {
        // Arrange
        var identifiqueSeRequestDto = new IdentifiqueSeRequestDto
        {
            Email = "teste@teste.com",
            Senha = "Senha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto.Email, identifiqueSeRequestDto.Senha, cancellationToken))
            .ReturnsAsync((TokenUsuario?)null);

        // Act
        var result = await _usuarioController.IdentificarUsuarioAsync(identifiqueSeRequestDto, cancellationToken);

        // Assert
        Assert.Null(result);
        _usuarioUseCaseMock.Verify(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto.Email, identifiqueSeRequestDto.Senha, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_Success()
    {
        // Arrange
        var confirmarEmailVerificacaoDto = new ConfirmarEmailVerificacaoDto
        {
            Email = "teste@teste.com",
            CodigoVerificacao = "123456"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.ConfirmarEmailVerificacaoAsync(It.IsAny<EmailVerificacao>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken);

        // Assert
        Assert.True(result);
        _usuarioUseCaseMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(It.IsAny<EmailVerificacao>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_Failure()
    {
        // Arrange
        var confirmarEmailVerificacaoDto = new ConfirmarEmailVerificacaoDto
        {
            Email = "teste@teste.com",
            CodigoVerificacao = "123456"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.ConfirmarEmailVerificacaoAsync(It.IsAny<EmailVerificacao>(), cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioController.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken);

        // Assert
        Assert.False(result);
        _usuarioUseCaseMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(It.IsAny<EmailVerificacao>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Success()
    {
        // Arrange
        var solicitarRecuperacaoSenhaDto = new SolicitarRecuperacaoSenhaDto
        {
            Email = "teste@teste.com"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.SolicitarRecuperacaoSenhaAsync(It.IsAny<RecuperacaoSenha>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken);

        // Assert
        Assert.True(result);
        _usuarioUseCaseMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(It.IsAny<RecuperacaoSenha>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Failure()
    {
        // Arrange
        var solicitarRecuperacaoSenhaDto = new SolicitarRecuperacaoSenhaDto
        {
            Email = "teste@teste.com"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.SolicitarRecuperacaoSenhaAsync(It.IsAny<RecuperacaoSenha>(), cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioController.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken);

        // Assert
        Assert.False(result);
        _usuarioUseCaseMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(It.IsAny<RecuperacaoSenha>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Success()
    {
        // Arrange
        var resetarSenhaDto = new ResetarSenhaDto
        {
            Email = "teste@teste.com",
            CodigoVerificacao = "123456",
            NovaSenha = "NovaSenha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.EfetuarResetSenhaAsync(It.IsAny<ResetSenha>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioController.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken);

        // Assert
        Assert.True(result);
        _usuarioUseCaseMock.Verify(x => x.EfetuarResetSenhaAsync(It.IsAny<ResetSenha>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Failure()
    {
        // Arrange
        var resetarSenhaDto = new ResetarSenhaDto
        {
            Email = "teste@teste.com",
            CodigoVerificacao = "123456",
            NovaSenha = "NovaSenha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioUseCaseMock
            .Setup(x => x.EfetuarResetSenhaAsync(It.IsAny<ResetSenha>(), cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioController.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken);

        // Assert
        Assert.False(result);
        _usuarioUseCaseMock.Verify(x => x.EfetuarResetSenhaAsync(It.IsAny<ResetSenha>(), cancellationToken), Times.Once);
    }
}