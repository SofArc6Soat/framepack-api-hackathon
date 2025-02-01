using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.Controller;
using Domain.Entities;
using Gateways.Dtos.Request;
using Gateways.Dtos.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Framepack_WebApi.Tests.External.Api.Controllers;

public class ConversoesApiControllerTests
{
    private readonly Mock<IConversaoController> _conversaoControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly ConversoesApiController _controller;

    public ConversoesApiControllerTests()
    {
        _conversaoControllerMock = new Mock<IConversaoController>();
        _notificadorMock = new Mock<INotificador>();
        _userContextServiceMock = new Mock<IUserContextService>();
        _controller = new ConversoesApiController(_conversaoControllerMock.Object, _notificadorMock.Object, _userContextServiceMock.Object);
    }

    [Fact]
    public async Task EfetuarUploadAsync_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("NomeArquivo", "Required");
        var request = new UploadRequestDto
        {
            NomeArquivo = "video.mp4",
            ArquivoVideo = Mock.Of<IFormFile>()
        };

        // Act
        var result = await _controller.EfetuarUploadAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(badRequestResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Success_ReturnsCustomResponsePost()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var usuarioId = "id-do-usuario";
        var content = "Hello World from a Fake File";
        var fileName = "video.mp4";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
        mockFile.Setup(_ => _.FileName).Returns(fileName);
        mockFile.Setup(_ => _.Length).Returns(ms.Length);

        var request = new UploadRequestDto
        {
            NomeArquivo = "video.mp4",
            ArquivoVideo = mockFile.Object
        };

        _conversaoControllerMock.Setup(x => x.EfetuarUploadAsync(request, usuarioId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.EfetuarUploadAsync(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<BaseApiResponse>(createdResult.Value);
        Assert.True(response.Success);
    }

    [Fact]
    public async Task ObterConversoesPorUsuarioAsync_Success_ReturnsCustomResponseGet()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var conversoes = new List<ObterCoversoesResult>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Data = DateTime.Now,
                    Status = "Completed",
                    NomeArquivo = "video.mp4",
                    UrlArquivoCompactado = "url"
                }
            };
        _userContextServiceMock.Setup(x => x.UserId).Returns(usuarioId);
        _conversaoControllerMock.Setup(x => x.ObterConversoesPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>())).ReturnsAsync(conversoes);

        // Act
        var result = await _controller.ObterConversoesPorUsuarioAsync(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BaseApiResponse>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(conversoes, response.Data);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_Success_ReturnsFile()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var conversaoId = Guid.NewGuid();
        var arquivo = new Arquivo(new byte[] { 1, 2, 3 }, "video.zip");
        _userContextServiceMock.Setup(x => x.UserId).Returns(usuarioId);
        _conversaoControllerMock.Setup(x => x.EfetuarDownloadAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>())).ReturnsAsync(arquivo);

        // Act
        var result = await _controller.EfetuarDownloadAsync(conversaoId, CancellationToken.None);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/zip", fileResult.ContentType);
        Assert.Equal("video.zip", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_NotFound_ReturnsNotFound()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var conversaoId = Guid.NewGuid();
        _userContextServiceMock.Setup(x => x.UserId).Returns(usuarioId);
        _conversaoControllerMock.Setup(x => x.EfetuarDownloadAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>())).ReturnsAsync((Arquivo)null);

        // Act
        var result = await _controller.EfetuarDownloadAsync(conversaoId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
