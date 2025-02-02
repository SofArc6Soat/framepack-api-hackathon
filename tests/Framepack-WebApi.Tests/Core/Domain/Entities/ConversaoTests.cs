using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Framepack_WebApi.Tests.Core.Domain.Entities
{
    public class ConversaoTests
    {
        [Fact]
        public void Conversao_DeveSerCriadaComValoresValidos()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuarioId = "id-do-usuario";
            var data = DateTime.Now;
            var status = Status.AguardandoConversao;
            var nomeArquivo = "video.mp4";
            var urlArquivoVideo = new Mock<IFormFile>();

            // Act
            var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, urlArquivoVideo.Object);

            // Assert
            Assert.Equal(id, conversao.Id);
            Assert.Equal(usuarioId, conversao.UsuarioId);
            Assert.Equal(data, conversao.Data);
            Assert.Equal(status, conversao.Status);
            Assert.Equal(nomeArquivo, conversao.NomeArquivo);
        }

        [Fact]
        public void ValidarConversao_DeveValidarComSucesso()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuarioId = "id-do-usuario";
            var data = DateTime.Now;
            var status = Status.AguardandoConversao;
            var nomeArquivo = "video.mp4";
            var urlArquivoVideo = new Mock<IFormFile>();

            var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, urlArquivoVideo.Object);
            var validator = new ValidarConversao();

            // Act
            var result = validator.Validate(conversao);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidarConversao_DeveFalharQuandoIdForInvalido()
        {
            // Arrange
            var usuarioId = "id-do-usuario";
            var data = DateTime.Now;
            var status = Status.AguardandoConversao;
            var nomeArquivo = "video.mp4";
            var urlArquivoVideo = new Mock<IFormFile>();
            var conversao = new Conversao(Guid.Empty, usuarioId, data, status, nomeArquivo, urlArquivoVideo.Object);
            var validator = new ValidarConversao();

            // Act
            var result = validator.Validate(conversao);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id");
        }

        [Fact]
        public void ValidarConversao_DeveFalharQuandoUsuarioIdForInvalido()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuarioId = string.Empty; // UsuarioId inválido
            var data = DateTime.Now;
            var status = Status.AguardandoConversao;
            var nomeArquivo = "video.mp4";
            var urlArquivoVideo = new Mock<IFormFile>();
            var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, urlArquivoVideo.Object);
            var validator = new ValidarConversao();

            // Act
            var result = validator.Validate(conversao);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UsuarioId");
        }

        [Fact]
        public void ValidarConversao_DeveFalharQuandoDataForInvalida()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuarioId = "id-do-usuario";
            var status = Status.AguardandoConversao;
            var nomeArquivo = "video.mp4";
            var urlArquivoVideo = new Mock<IFormFile>();
            var conversao = new Conversao(id, usuarioId, default, status, nomeArquivo, urlArquivoVideo.Object);
            var validator = new ValidarConversao();

            // Act
            var result = validator.Validate(conversao);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Data");
        }

        [Fact]
        public void ValidarConversao_DeveFalharQuandoNomeArquivoForInvalido()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuarioId = "id-do-usuario";
            var data = DateTime.Now;
            var status = Status.AguardandoConversao;
            var nomeArquivo = "a"; // NomeArquivo com menos de 2 caracteres
            var urlArquivoVideo = new Mock<IFormFile>();
            var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, urlArquivoVideo.Object);
            var validator = new ValidarConversao();

            // Act
            var result = validator.Validate(conversao);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "NomeArquivo");
        }
    }
}
