using Controllers;
using Domain.Entities;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Http;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Adpters.Controllers;

public class ConversaoControllerTests
{
    private readonly Mock<IConversaoUseCase> _conversaoUseCaseMock;
    private readonly ConversaoController _conversaoController;

    public ConversaoControllerTests()
    {
        _conversaoUseCaseMock = new Mock<IConversaoUseCase>();
        _conversaoController = new ConversaoController(_conversaoUseCaseMock.Object);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Success()
    {
        // Arrange
        var uploadRequestDto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "video.mp4",
            ArquivoVideo = new FormFile(null, 0, 0, null, "video.mp4")
        };
        var cancellationToken = CancellationToken.None;

        _conversaoUseCaseMock
            .Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _conversaoController.EfetuarUploadAsync(uploadRequestDto, cancellationToken);

        // Assert
        Assert.True(result);
        _conversaoUseCaseMock.Verify(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Failure()
    {
        // Arrange
        var uploadRequestDto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "video.mp4",
            ArquivoVideo = new FormFile(null, 0, 0, null, "video.mp4")
        };
        var cancellationToken = CancellationToken.None;

        _conversaoUseCaseMock
            .Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _conversaoController.EfetuarUploadAsync(uploadRequestDto, cancellationToken);

        // Assert
        Assert.False(result);
        _conversaoUseCaseMock.Verify(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ThrowsException()
    {
        // Arrange
        var uploadRequestDto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "video.mp4",
            ArquivoVideo = new FormFile(null, 0, 0, null, "video.mp4")
        };
        var cancellationToken = CancellationToken.None;

        _conversaoUseCaseMock
            .Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), cancellationToken))
            .ThrowsAsync(new Exception("Erro ao efetuar upload"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _conversaoController.EfetuarUploadAsync(uploadRequestDto, cancellationToken));
        _conversaoUseCaseMock.Verify(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), cancellationToken), Times.Once);
    }
}