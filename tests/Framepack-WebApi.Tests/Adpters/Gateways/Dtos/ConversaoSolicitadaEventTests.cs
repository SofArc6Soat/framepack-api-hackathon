using Gateways.Dtos.Events;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Dtos;

public class ConversaoSolicitadaEventTests
{
    [Fact]
    public void ConversaoSolicitadaEvent_ShouldInitializeWithDefaultValues()
    {
        // Act
        var evento = new ConversaoSolicitadaEvent();

        // Assert
        Assert.Equal(string.Empty, evento.UsuarioId);
        Assert.Equal(default(DateTime), evento.Data);
        Assert.Equal(string.Empty, evento.Status);
        Assert.Equal(string.Empty, evento.NomeArquivo);
        Assert.Equal(string.Empty, evento.UrlArquivoVideo);
    }

    [Fact]
    public void ConversaoSolicitadaEvent_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var usuarioId = "id-do-usuario";
        var data = DateTime.UtcNow;
        var status = "Iniciado";
        var nomeArquivo = "video.mp4";
        var urlArquivoVideo = "http://example.com/video.mp4";

        // Act
        var evento = new ConversaoSolicitadaEvent
        {
            UsuarioId = usuarioId,
            Data = data,
            Status = status,
            NomeArquivo = nomeArquivo,
            UrlArquivoVideo = urlArquivoVideo
        };

        // Assert
        Assert.Equal(usuarioId, evento.UsuarioId);
        Assert.Equal(data, evento.Data);
        Assert.Equal(status, evento.Status);
        Assert.Equal(nomeArquivo, evento.NomeArquivo);
        Assert.Equal(urlArquivoVideo, evento.UrlArquivoVideo);
    }

    [Fact]
    public void ConversaoSolicitadaEvent_ShouldAcceptMinValueForData()
    {
        // Arrange
        var evento = new ConversaoSolicitadaEvent
        {
            // Act
            Data = DateTime.MinValue
        };

        // Assert
        Assert.Equal(DateTime.MinValue, evento.Data);
    }
}
