using Infra.Dto;

namespace Framepack_WebApi.Tests.External.Infra.Dto;

public class ConversaoDbTests
{
    [Fact]
    public void ConversaoDb_DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var conversaoDb = new ConversaoDb();

        // Assert
        Assert.Equal(Guid.Empty, conversaoDb.Id);
        Assert.Equal(string.Empty, conversaoDb.UsuarioId);
        Assert.Equal(string.Empty, conversaoDb.Status);
        Assert.Equal(default(DateTime), conversaoDb.Data);
        Assert.Equal(string.Empty, conversaoDb.UrlArquivoVideo);
        Assert.Equal(string.Empty, conversaoDb.UrlArquivoCompactado);
    }

    [Fact]
    public void ConversaoDb_SetValues_ShouldBeSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = "id-do-usuario";
        var status = "Completed";
        var data = DateTime.UtcNow;
        var urlArquivoVideo = "http://example.com/video.mp4";
        var urlArquivoCompactado = "http://example.com/video.zip";

        // Act
        var conversaoDb = new ConversaoDb
        {
            Id = id,
            UsuarioId = usuarioId,
            Status = status,
            Data = data,
            UrlArquivoVideo = urlArquivoVideo,
            UrlArquivoCompactado = urlArquivoCompactado
        };

        // Assert
        Assert.Equal(id, conversaoDb.Id);
        Assert.Equal(usuarioId, conversaoDb.UsuarioId);
        Assert.Equal(status, conversaoDb.Status);
        Assert.Equal(data, conversaoDb.Data);
        Assert.Equal(urlArquivoVideo, conversaoDb.UrlArquivoVideo);
        Assert.Equal(urlArquivoCompactado, conversaoDb.UrlArquivoCompactado);
    }

    [Fact]
    public void ConversaoDb_NullableProperties_ShouldHandleNullValues()
    {
        // Arrange & Act
        var conversaoDb = new ConversaoDb
        {
            UrlArquivoVideo = null,
            UrlArquivoCompactado = null
        };

        // Assert
        Assert.Null(conversaoDb.UrlArquivoVideo);
        Assert.Null(conversaoDb.UrlArquivoCompactado);
    }
}