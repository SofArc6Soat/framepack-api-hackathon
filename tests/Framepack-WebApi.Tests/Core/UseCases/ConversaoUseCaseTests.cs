using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;

using Gateways;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Core.UseCases;

public class ConversaoUseCaseTests
{
    private readonly Mock<IConversaoGateway> _conversaoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly ConversaoUseCase _conversaoUseCase;

    public ConversaoUseCaseTests()
    {
        _conversaoGatewayMock = new Mock<IConversaoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _conversaoUseCase = new ConversaoUseCase(_conversaoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ShouldThrowArgumentNullException_WhenConversaoIsNull()
    {
        // Arrange
        Conversao? conversao = null;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _conversaoUseCase.EfetuarUploadAsync(conversao, cancellationToken));
    }

    [Fact]
    public async Task EfetuarUploadAsync_ShouldReturnTrue_WhenUploadIsSuccessful()
    {
        // Arrange
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Status.AguardandoConversao, "video.mp4", null);
        var cancellationToken = CancellationToken.None;

        _conversaoGatewayMock.Setup(x => x.EfetuarUploadAsync(conversao, cancellationToken)).ReturnsAsync(true);

        // Act
        var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ShouldReturnFalse_WhenUploadFails()
    {
        // Arrange
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Status.AguardandoConversao, "video.mp4", null);
        var cancellationToken = CancellationToken.None;

        _conversaoGatewayMock.Setup(x => x.EfetuarUploadAsync(conversao, cancellationToken)).ReturnsAsync(false);

        // Act
        var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, cancellationToken);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.Is<Notificacao>(n => n.Mensagem == "Ocorreu um erro ao efetuar o upload do vídeo.")), Times.Once);
    }
}