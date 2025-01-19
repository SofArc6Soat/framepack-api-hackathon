using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Dtos.Response;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Core.UseCases;

public class UsuarioUseCaseTests
{
    private readonly Mock<ICognitoGateway> _cognitoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly UsuarioUseCase _usuarioUseCase;

    public UsuarioUseCaseTests()
    {
        _cognitoGatewayMock = new Mock<ICognitoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _usuarioUseCase = new UsuarioUseCase(_cognitoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task CadastrarUsuarioAsync_DeveRetornarTrue_QuandoUsuarioForCadastradoComSucesso()
    {
        // Arrange
        var usuario = new Usuario(Guid.NewGuid(), "Nome", "email@teste.com");
        var senha = "senha123";
        var cancellationToken = CancellationToken.None;

        _cognitoGatewayMock.Setup(x => x.CriarUsuarioAsync(usuario, senha, cancellationToken)).ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.CadastrarUsuarioAsync(usuario, senha, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarUsuarioAsync_DeveRetornarFalse_QuandoUsuarioNaoForCadastrado()
    {
        // Arrange
        var usuario = new Usuario(Guid.NewGuid(), "Nome", "email@teste.com");
        var senha = "senha123";
        var cancellationToken = CancellationToken.None;

        _cognitoGatewayMock.Setup(x => x.CriarUsuarioAsync(usuario, senha, cancellationToken)).ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.CadastrarUsuarioAsync(usuario, senha, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IdentificarUsuarioAsync_DeveRetornarTokenUsuario_QuandoCredenciaisForemValidas()
    {
        // Arrange
        var email = "email@teste.com";
        var senha = "senha123";
        var cancellationToken = CancellationToken.None;
        var tokenUsuario = new TokenUsuario { Email = email, AccessToken = "accessToken", RefreshToken = "refreshToken", Expiry = DateTimeOffset.Now.AddHours(1) };

        _cognitoGatewayMock.Setup(x => x.IdentifiqueSeAsync(email, senha, cancellationToken)).ReturnsAsync(tokenUsuario);

        // Act
        var result = await _usuarioUseCase.IdentificarUsuarioAsync(email, senha, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarFalse_QuandoValidacaoFalhar()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("email@teste.com", "codigo123");
        var cancellationToken = CancellationToken.None;

        _cognitoGatewayMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken)).ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarTrue_QuandoSolicitacaoForRealizadaComSucesso()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("email@teste.com");
        var cancellationToken = CancellationToken.None;

        _cognitoGatewayMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken)).ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarFalse_QuandoValidacaoFalhar()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("email@teste.com");
        var cancellationToken = CancellationToken.None;

        _cognitoGatewayMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken)).ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarFalse_QuandoValidacaoFalhar()
    {
        // Arrange
        var resetSenha = new ResetSenha("email@teste.com", "codigo123", "novaSenha123");
        var cancellationToken = CancellationToken.None;

        _cognitoGatewayMock.Setup(x => x.EfetuarResetSenhaAsync(resetSenha, cancellationToken)).ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.EfetuarResetSenhaAsync(resetSenha, cancellationToken);

        // Assert
        Assert.False(result);
    }
}