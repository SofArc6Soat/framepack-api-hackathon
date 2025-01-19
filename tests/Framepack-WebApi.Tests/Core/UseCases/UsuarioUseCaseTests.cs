using Core.Domain.Notificacoes;
using Domain.ValueObjects;
using Gateways.Cognito;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Core.UseCases;

public class UsuarioUseCaseTests
{
    private readonly Mock<ICognitoGateway> _cognitoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly IUsuarioUseCase _usuarioUseCase;

    public UsuarioUseCaseTests()
    {
        _cognitoGatewayMock = new Mock<ICognitoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _usuarioUseCase = new UsuarioUseCase(_cognitoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarTrue_QuandoConfirmacaoBemSucedida()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@example.com", "codigo123");

        _cognitoGatewayMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.True(result);
        _cognitoGatewayMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_DeveRetornarFalse_QuandoConfirmacaoFalhar()
    {
        // Arrange
        var emailVerificacao = new EmailVerificacao("usuario@example.com", "codigo123");

        _cognitoGatewayMock.Setup(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        // Assert
        Assert.False(result);
        _cognitoGatewayMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(emailVerificacao, It.IsAny<CancellationToken>()), Times.Once);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarTrue_QuandoSolicitacaoBemSucedida()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@example.com");

        _cognitoGatewayMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.True(result);
        _cognitoGatewayMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_DeveRetornarFalse_QuandoSolicitacaoFalhar()
    {
        // Arrange
        var recuperacaoSenha = new RecuperacaoSenha("usuario@example.com");

        _cognitoGatewayMock.Setup(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _cognitoGatewayMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, It.IsAny<CancellationToken>()), Times.Once);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarTrue_QuandoResetBemSucedido()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@example.com", "codigo123", "novaSenha123");

        _cognitoGatewayMock.Setup(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _usuarioUseCase.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.True(result);
        _cognitoGatewayMock.Verify(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_DeveRetornarFalse_QuandoResetFalhar()
    {
        // Arrange
        var resetSenha = new ResetSenha("usuario@example.com", "codigo123", "novaSenha123");

        _cognitoGatewayMock.Setup(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _usuarioUseCase.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        // Assert
        Assert.False(result);
        _cognitoGatewayMock.Verify(x => x.EfetuarResetSenhaAsync(resetSenha, It.IsAny<CancellationToken>()), Times.Once);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }
}