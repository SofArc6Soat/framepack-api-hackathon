using Amazon.SQS;
using Amazon.SQS.Model;
using Core.Infra.MessageBroker;
using Moq;
using System.Text.Json;

namespace Framepack_WebApi.Tests.Core.Infra.MessageBroker
{
    public class SqsServiceTests
    {
        private readonly Mock<IAmazonSQS> _sqsClientMock;
        private readonly SqsService<TestMessage> _sqsService;
        private readonly string _queueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue";

        public SqsServiceTests()
        {
            _sqsClientMock = new Mock<IAmazonSQS>();
            _sqsService = new SqsService<TestMessage>(_sqsClientMock.Object, _queueUrl);
        }

        [Fact]
        public async Task SendMessageAsync_DeveRetornarTrue_QuandoMensagemEnviadaComSucesso()
        {
            // Arrange
            var testMessage = new TestMessage { Id = 1, Content = "Test Content" };
            var sendMessageResponse = new SendMessageResponse { MessageId = "12345" };
            _sqsClientMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync(sendMessageResponse);

            // Act
            var result = await _sqsService.SendMessageAsync(testMessage);

            // Assert
            Assert.True(result);
            _sqsClientMock.Verify(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_DeveRetornarFalse_QuandoMensagemNaoEnviada()
        {
            // Arrange
            _sqsClientMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync((SendMessageResponse)null);

            // Act
            var result = await _sqsService.SendMessageAsync(new TestMessage());

            // Assert
            Assert.False(result);
            _sqsClientMock.Verify(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_DeveRetornarObjeto_QuandoMensagemRecebidaComSucesso()
        {
            // Arrange
            var testMessage = new TestMessage { Id = 1, Content = "Test Content" };
            var messageBody = JsonSerializer.Serialize(testMessage);
            var receiveMessageResponse = new ReceiveMessageResponse
            {
                Messages =
                    [
                        new Message { Body = messageBody, ReceiptHandle = "receipt-handle" }
                    ]
            };
            _sqsClientMock.Setup(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
                .ReturnsAsync(receiveMessageResponse);
            _sqsClientMock.Setup(x => x.DeleteMessageAsync(_queueUrl, "receipt-handle", default))
                .ReturnsAsync(new DeleteMessageResponse());

            // Act
            var result = await _sqsService.ReceiveMessagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testMessage.Id, result.Id);
            Assert.Equal(testMessage.Content, result.Content);
            _sqsClientMock.Verify(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default), Times.Once);
            _sqsClientMock.Verify(x => x.DeleteMessageAsync(_queueUrl, "receipt-handle", default), Times.Once);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_DeveRetornarNull_QuandoNenhumaMensagemRecebida()
        {
            // Arrange
            var receiveMessageResponse = new ReceiveMessageResponse { Messages = [] };
            _sqsClientMock.Setup(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
                .ReturnsAsync(receiveMessageResponse);

            // Act
            var result = await _sqsService.ReceiveMessagesAsync();

            // Assert
            Assert.Null(result);
            _sqsClientMock.Verify(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_DeveRetornarNull_QuandoJsonInvalido()
        {
            // Arrange
            var invalidMessageBody = "Invalid JSON";
            var receiveMessageResponse = new ReceiveMessageResponse
            {
                Messages =
                    [
                        new Message { Body = invalidMessageBody, ReceiptHandle = "receipt-handle" }
                    ]
            };
            _sqsClientMock.Setup(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
                .ReturnsAsync(receiveMessageResponse);

            // Act
            var result = await _sqsService.ReceiveMessagesAsync();

            // Assert
            Assert.Null(result);
            _sqsClientMock.Verify(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default), Times.Once);
        }

        private class TestMessage
        {
            public int Id { get; set; }
            public string Content { get; set; }
        }
    }
}