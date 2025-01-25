using Gateways.Dtos.Events;

namespace Framepack_WebApi.Tests.Adpters.Gateways
{
    public class ConversaoCriadaTests
    {
        [Fact]
        public void ConversaoCriada_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var conversaoCriada = new ConversaoSolicitadaEvent();

            // Assert
            Assert.Equal(Guid.Empty, conversaoCriada.UsuarioId);
            Assert.Equal(DateTime.Now, conversaoCriada.Data);
            Assert.Equal(string.Empty, conversaoCriada.Status);
            Assert.Equal(string.Empty, conversaoCriada.NomeArquivo);
            Assert.Equal(string.Empty, conversaoCriada.UrlArquivoVideo);
        }

        [Fact]
        public void ConversaoCriada_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var data = DateTime.Now;
            var status = "Completed";
            var nomeArquivo = "video.mp4";
            var urlArquivoVideo = "http://example.com/video.mp4";

            // Act
            var conversaoCriada = new ConversaoSolicitadaEvent
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
}