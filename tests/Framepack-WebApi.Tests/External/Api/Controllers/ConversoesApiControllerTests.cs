using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Framepack_WebApi.Tests.External.Api.Controllers;

public class ConversoesApiControllerTests
{
    private readonly Mock<IConversaoController> _conversaoControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly ConversoesApiController _controller;

    public ConversoesApiControllerTests()
    {
        _conversaoControllerMock = new Mock<IConversaoController>();
        _notificadorMock = new Mock<INotificador>();
        _controller = new ConversoesApiController(_conversaoControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("NomeArquivo", "O campo NomeArquivo é obrigatório.");
        var request = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "video.mp4",
            ArquivoVideo = CreateMockFormFile()
        };

        // Act
        var result = await _controller.EfetuarUploadAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var baseApiResponse = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(baseApiResponse.Success);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ConversaoControllerReturnsFalse_ReturnsCreatedResult()
    {
        // Arrange
        var request = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "video.mp4",
            ArquivoVideo = CreateMockFormFile()
        };
        _conversaoControllerMock.Setup(c => c.EfetuarUploadAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _controller.EfetuarUploadAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ConversaoControllerReturnsTrue_ReturnsCreatedResult()
    {
        // Arrange
        var request = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "video.mp4",
            ArquivoVideo = CreateMockFormFile()
        };
        _conversaoControllerMock.Setup(c => c.EfetuarUploadAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.EfetuarUploadAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    private IFormFile CreateMockFormFile()
    {
        var fileMock = new Mock<IFormFile>();
        var content = "Hello World from a Fake File";
        var fileName = "video.mp4";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        fileMock.Setup(_ => _.ContentType).Returns("video/mp4");

        return fileMock.Object;
    }
}
