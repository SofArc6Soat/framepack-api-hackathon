using Controllers;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Http;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Adpters.Controllers;

public class ConversaoControllerTests
{
    private readonly Mock<IConversaoUseCase> _conversaoUseCaseMock;
    private readonly ConversaoController _controller;

    public ConversaoControllerTests()
    {
        _conversaoUseCaseMock = new Mock<IConversaoUseCase>();
        _controller = new ConversaoController(_conversaoUseCaseMock.Object);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Success()
    {
        // Arrange
        var uploadRequestDto = new UploadRequestDto
        {
            NomeArquivo = "video.mp4",
            ArquivoVideo = Mock.Of<IFormFile>()
        };
        _conversaoUseCaseMock.Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var usuarioId = "id-do-usuario";

        // Act
        var result = await _controller.EfetuarUploadAsync(uploadRequestDto, usuarioId, CancellationToken.None);

        // Assert
        Assert.True(result);
        _conversaoUseCaseMock.Verify(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Failure()
    {
        // Arrange
        var uploadRequestDto = new UploadRequestDto
        {
            NomeArquivo = "video.mp4",
            ArquivoVideo = Mock.Of<IFormFile>()
        };
        _conversaoUseCaseMock.Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var usuarioId = "id-do-usuario";

        // Act
        var result = await _controller.EfetuarUploadAsync(uploadRequestDto, usuarioId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _conversaoUseCaseMock.Verify(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterConversoesPorUsuarioAsync_Success()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var conversoes = new List<Conversao>
            {
                new(Guid.NewGuid(), usuarioId, DateTime.Now, Status.AguardandoConversao, "video.mp4", "urlVideo", "urlCompactado")
            };
        _conversaoUseCaseMock.Setup(x => x.ObterConversoesPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversoes);

        // Act
        var result = await _controller.ObterConversoesPorUsuarioAsync(usuarioId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _conversaoUseCaseMock.Verify(x => x.ObterConversoesPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterConversoesPorUsuarioAsync_NoConversoes()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        _conversaoUseCaseMock.Setup(x => x.ObterConversoesPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Conversao>?)null);

        // Act
        var result = await _controller.ObterConversoesPorUsuarioAsync(usuarioId, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _conversaoUseCaseMock.Verify(x => x.ObterConversoesPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_Success()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var conversaoId = Guid.NewGuid();
        var arquivo = new Arquivo(new byte[] { 1, 2, 3 }, "video.mp4");
        _conversaoUseCaseMock.Setup(x => x.EfetuarDownloadAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(arquivo);

        // Act
        var result = await _controller.EfetuarDownloadAsync(usuarioId, conversaoId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(arquivo, result);
        _conversaoUseCaseMock.Verify(x => x.EfetuarDownloadAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_Failure()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var conversaoId = Guid.NewGuid();
        _conversaoUseCaseMock.Setup(x => x.EfetuarDownloadAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Arquivo?)null);

        // Act
        var result = await _controller.EfetuarDownloadAsync(usuarioId, conversaoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _conversaoUseCaseMock.Verify(x => x.EfetuarDownloadAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
