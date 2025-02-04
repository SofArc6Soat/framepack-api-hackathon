using Amazon.CognitoIdentityProvider.Model;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;

using Gateways;
using Gateways.Cognito;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Core.UseCases
{
    public class ConversaoUseCaseTests
    {
        private readonly Mock<IConversaoGateway> _conversaoGatewayMock;
        private readonly Mock<ICognitoGateway> _cognitoGatewayMock;
        private readonly Mock<INotificador> _notificadorMock;
        private readonly ConversaoUseCase _conversaoUseCase;

        public ConversaoUseCaseTests()
        {
            _conversaoGatewayMock = new Mock<IConversaoGateway>();
            _cognitoGatewayMock = new Mock<ICognitoGateway>();
            _notificadorMock = new Mock<INotificador>();
            _conversaoUseCase = new ConversaoUseCase(_conversaoGatewayMock.Object, _cognitoGatewayMock.Object, _notificadorMock.Object);
        }

        [Fact]
        public async Task EfetuarUploadAsync_DeveRetornarFalse_QuandoConversaoForNula()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _conversaoUseCase.EfetuarUploadAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task EfetuarUploadAsync_DeveRetornarFalse_QuandoEmailUsuarioNaoForEncontrado()
        {
            var conversao = new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", "urlArquivoCompactado");
            _cognitoGatewayMock.Setup(x => x.ObertUsuarioCognitoPorIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AdminGetUserResponse { UserAttributes = new List<AttributeType>() });

            var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, CancellationToken.None);

            Assert.False(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task EfetuarUploadAsync_DeveRetornarTrue_QuandoUploadForBemSucedido()
        {
            var conversao = new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", "urlArquivoCompactado");
            var userAttributes = new List<AttributeType> { new AttributeType { Name = "email", Value = "email@teste.com" } };
            _cognitoGatewayMock.Setup(x => x.ObertUsuarioCognitoPorIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AdminGetUserResponse { UserAttributes = userAttributes });
            _conversaoGatewayMock.Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task EfetuarUploadAsync_DeveRetornarFalse_QuandoUploadFalhar()
        {
            var conversao = new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", "urlArquivoCompactado");
            var userAttributes = new List<AttributeType> { new AttributeType { Name = "email", Value = "email@teste.com" } };
            _cognitoGatewayMock.Setup(x => x.ObertUsuarioCognitoPorIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AdminGetUserResponse { UserAttributes = userAttributes });
            _conversaoGatewayMock.Setup(x => x.EfetuarUploadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, CancellationToken.None);

            Assert.False(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task ObterConversoesPorUsuarioAsync_DeveRetornarListaDeConversoes()
        {
            var conversoes = new List<Conversao> { new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", "urlArquivoCompactado") };
            _conversaoGatewayMock.Setup(x => x.ObterConversoesPorUsuarioAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(conversoes);

            var result = await _conversaoUseCase.ObterConversoesPorUsuarioAsync("usuarioId", CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task EfetuarDownloadAsync_DeveRetornarNull_QuandoConversaoNaoExistir()
        {
            _conversaoGatewayMock.Setup(x => x.ObterConversaoAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Conversao)null);

            var result = await _conversaoUseCase.EfetuarDownloadAsync("usuarioId", Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task EfetuarDownloadAsync_DeveRetornarNull_QuandoUrlArquivoCompactadoForNula()
        {
            var conversao = new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", null);
            _conversaoGatewayMock.Setup(x => x.ObterConversaoAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(conversao);

            var result = await _conversaoUseCase.EfetuarDownloadAsync("usuarioId", Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task EfetuarDownloadAsync_DeveRetornarNull_QuandoArquivoDownloadForNulo()
        {
            var conversao = new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", "urlArquivoCompactado");
            _conversaoGatewayMock.Setup(x => x.ObterConversaoAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(conversao);
            _conversaoGatewayMock.Setup(x => x.EfetuarDownloadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Arquivo)null);

            var result = await _conversaoUseCase.EfetuarDownloadAsync("usuarioId", Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
            _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public async Task EfetuarDownloadAsync_DeveRetornarArquivo_QuandoDownloadForBemSucedido()
        {
            var conversao = new Conversao(Guid.NewGuid(), "usuarioId", DateTime.Now, Status.AguardandoConversao, "nomeArquivo", "urlArquivoVideo", "urlArquivoCompactado");
            var arquivo = new Arquivo(new byte[] { 1, 2, 3 }, "arquivo.zip");
            _conversaoGatewayMock.Setup(x => x.ObterConversaoAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(conversao);
            _conversaoGatewayMock.Setup(x => x.EfetuarDownloadAsync(It.IsAny<Conversao>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(arquivo);

            var result = await _conversaoUseCase.EfetuarDownloadAsync("usuarioId", Guid.NewGuid(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("arquivo.zip", result.NomeArquivo);
        }
    }
}
