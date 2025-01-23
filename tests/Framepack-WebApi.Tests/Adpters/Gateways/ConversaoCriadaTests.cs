using Gateways.Dtos.Events;

namespace Framepack_WebApi.Tests.Adpters.Gateways;

public class ConversaoCriadaTests
{
    [Fact]
    public void ConversaoCriada_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var conversaoCriada = new ConversaoCriada();

        // Assert
        Assert.Equal(Guid.Empty, conversaoCriada.UsuarioId);
        Assert.Equal(string.Empty, conversaoCriada.Data);
        Assert.Equal(string.Empty, conversaoCriada.Status);
        Assert.Equal(string.Empty, conversaoCriada.NomeArquivo);
        Assert.Equal(string.Empty, conversaoCriada.UrlArquivoVideo);
    }

    [Fact]
    public void ConversaoCriada_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var data = "2023-10-01";
        var status = "Completed";
        var nomeArquivo = "video.mp4";
        var urlArquivoVideo = "http://example.com/video.mp4";

        // Act
        var conversaoCriada = new ConversaoCriada
        {
            UsuarioId = usuarioId,
            Data = data,
            Status = status,
            NomeArquivo = nomeArquivo,
            UrlArquivoVideo = urlArquivoVideo
        };

        // Assert
        Assert.Equal(usuarioId, conversaoCriada.UsuarioId);
        Assert.Equal(data, conversaoCriada.Data);
        Assert.Equal(status, conversaoCriada.Status);
        Assert.Equal(nomeArquivo, conversaoCriada.NomeArquivo);
        Assert.Equal(urlArquivoVideo, conversaoCriada.UrlArquivoVideo);
    }
}