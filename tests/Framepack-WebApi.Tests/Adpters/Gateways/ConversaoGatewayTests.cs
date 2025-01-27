using Amazon.DynamoDBv2.DataModel;
using Core.Infra.MessageBroker;
using Core.Infra.S3;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways;
using Gateways.Dtos.Events;
using Infra.Dto;
using Moq;
using Moq.Protected;
using System.Net;

namespace Framepack_WebApi.Tests.Adpters.Gateways
{
    public class ConversaoGatewayTests
    {
        private readonly Mock<IDynamoDBContext> _repositoryMock;
        private readonly Mock<ISqsService<ConversaoSolicitadaEvent>> _sqsServiceMock;
        private readonly Mock<IS3Service> _s3ServiceMock;
        private readonly ConversaoGateway _conversaoGateway;

        public ConversaoGatewayTests()
        {
            _repositoryMock = new Mock<IDynamoDBContext>();
            _sqsServiceMock = new Mock<ISqsService<ConversaoSolicitadaEvent>>();
            _s3ServiceMock = new Mock<IS3Service>();
            _conversaoGateway = new ConversaoGateway(_repositoryMock.Object, _sqsServiceMock.Object, _s3ServiceMock.Object);
        }

        [Fact]
        public async Task EfetuarUploadAsync_Success()
        {
            // Arrange
            var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.AguardandoConversao, "video.mp4", null);
            _s3ServiceMock.Setup(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo)).ReturnsAsync("http://s3.com/video.mp4");
            _repositoryMock.Setup(r => r.SaveAsync(It.IsAny<ConversaoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _sqsServiceMock.Setup(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>())).ReturnsAsync(true);

            // Act
            var result = await _conversaoGateway.EfetuarUploadAsync(conversao, CancellationToken.None);

            // Assert
            Assert.True(result);
            _s3ServiceMock.Verify(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo), Times.Once);
            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversaoDb>(), It.IsAny<CancellationToken>()));
            _sqsServiceMock.Verify(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>()));
        }

        [Fact]
        public async Task EfetuarUploadAsync_Failure_UploadError()
        {
            // Arrange
            var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.AguardandoConversao, "video.mp4", null);
            _s3ServiceMock.Setup(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo)).ReturnsAsync(string.Empty);

            // Act
            var result = await _conversaoGateway.EfetuarUploadAsync(conversao, CancellationToken.None);

            // Assert
            Assert.False(result);
            _s3ServiceMock.Verify(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo), Times.Once);
            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversaoDb>(), It.IsAny<CancellationToken>()), Times.Never);
            _sqsServiceMock.Verify(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>()), Times.Never);
        }

        [Fact]
        public async Task ObterConversoesPorUsuarioAsync_Success()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var conversaoDbList = new List<ConversaoDb>
                {
                    new ConversaoDb { Id = Guid.NewGuid(), UsuarioId = usuarioId, Status = "AguardandoConversao", Data = DateTime.UtcNow, NomeArquivo = "video.mp4", UrlArquivoVideo = "http://s3.com/video.mp4" }
                };
            var asyncSearchMock = new Mock<AsyncSearch<ConversaoDb>>();
            asyncSearchMock.Setup(a => a.GetRemainingAsync(It.IsAny<CancellationToken>())).ReturnsAsync(conversaoDbList);
            _repositoryMock.Setup(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>())).Returns(asyncSearchMock.Object);

            // Act
            var result = await _conversaoGateway.ObterConversoesPorUsuarioAsync(usuarioId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _repositoryMock.Verify(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>()), Times.Once);
        }

        [Fact]
        public async Task ObterConversoesPorUsuarioAsync_EmptyResult()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var asyncSearchMock = new Mock<AsyncSearch<ConversaoDb>>();
            asyncSearchMock.Setup(a => a.GetRemainingAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ConversaoDb>());
            _repositoryMock.Setup(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>())).Returns(asyncSearchMock.Object);

            // Act
            var result = await _conversaoGateway.ObterConversoesPorUsuarioAsync(usuarioId, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>()), Times.Once);
        }

        [Fact]
        public async Task EfetuarDownloadAsync_Failure()
        {
            // Arrange
            var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.Concluido, "video.mp4", "http://s3.com/video.mp4", "http://s3.com/video.zip");
            _s3ServiceMock.Setup(s => s.GerarPreSignedUrl(conversao.UrlArquivoCompactado, It.IsAny<int>())).Returns("http://s3.com/video.zip");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _conversaoGateway.EfetuarDownloadAsync(conversao, CancellationToken.None));
            _s3ServiceMock.Verify(s => s.GerarPreSignedUrl(conversao.UrlArquivoCompactado, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ObterConversaoAsync_Success()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var conversaoId = Guid.NewGuid();
            var conversaoDbList = new List<ConversaoDb>
                {
                    new ConversaoDb { Id = conversaoId, UsuarioId = usuarioId, Status = "AguardandoConversao", Data = DateTime.UtcNow, NomeArquivo = "video.mp4", UrlArquivoVideo = "http://s3.com/video.mp4" }
                };
            var asyncSearchMock = new Mock<AsyncSearch<ConversaoDb>>();
            asyncSearchMock.Setup(a => a.GetRemainingAsync(It.IsAny<CancellationToken>())).ReturnsAsync(conversaoDbList);
            _repositoryMock.Setup(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>())).Returns(asyncSearchMock.Object);

            // Act
            var result = await _conversaoGateway.ObterConversaoAsync(usuarioId, conversaoId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(conversaoId, result.Id);
            _repositoryMock.Verify(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>()), Times.Once);
        }

        [Fact]
        public async Task ObterConversaoAsync_NotFound()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var conversaoId = Guid.NewGuid();
            var asyncSearchMock = new Mock<AsyncSearch<ConversaoDb>>();
            asyncSearchMock.Setup(a => a.GetRemainingAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ConversaoDb>());
            _repositoryMock.Setup(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>())).Returns(asyncSearchMock.Object);

            // Act
            var result = await _conversaoGateway.ObterConversaoAsync(usuarioId, conversaoId, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.ScanAsync<ConversaoDb>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>()), Times.Once);
        }
    }
}