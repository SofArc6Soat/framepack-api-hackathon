using Gateways.Dtos.Events;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Dtos;

public class DownloadEfetuadoEventTests
{
    [Fact]
    public void DownloadEfetuadoEvent_ShouldInitializeWithDefaultValues()
    {
        // Act
        var downloadEvent = new DownloadEfetuadoEvent();

        // Assert
        Assert.Equal(string.Empty, downloadEvent.UrlArquivoVideo);
    }

    [Fact]
    public void DownloadEfetuadoEvent_ShouldSetUrlArquivoVideo()
    {
        // Arrange
        var expectedUrl = "http://example.com/video.mp4";

        // Act
        var downloadEvent = new DownloadEfetuadoEvent
        {
            UrlArquivoVideo = expectedUrl
        };

        // Assert
        Assert.Equal(expectedUrl, downloadEvent.UrlArquivoVideo);
    }

    [Fact]
    public void DownloadEfetuadoEvent_ShouldThrowException_WhenUrlArquivoVideoIsNull() =>
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DownloadEfetuadoEvent
        {
            UrlArquivoVideo = null
        });
}