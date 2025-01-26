

using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Core.Infra.MessageBroker;
using Core.Infra.S3;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways;
using Gateways.Dtos.Events;
using Infra.Dto;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Framepack_WebApi.Tests.Adpters.Gateways;
public class ConversaoGatewayTests
{
    private readonly Mock<IDynamoDBContext> _repositoryMock;
    private readonly Mock<ISqsService<ConversaoSolicitadaEvent>> _sqsServiceMock;
    private readonly Mock<IS3Service> _s3ServiceMock;
    private readonly IConversaoGateway _conversaoGateway;

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
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.AguardandoConversao, "video.mp4", new Mock<IFormFile>().Object);
        var cancellationToken = CancellationToken.None;
        var urlArquivoVideo = "https://amzn-s3-bucket-26bda3ac-c185-4185-a9f8-d3697a89754c-framepack.s3.amazonaws.com/aguardando-processamento/video.mp4";
        var preSignedUrl = "https://s3.amazonaws.com/bucket/video.mp4?signature=abc123";

        _s3ServiceMock.Setup(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo)).ReturnsAsync(urlArquivoVideo);
        _s3ServiceMock.Setup(s => s.GerarPreSignedUrl(urlArquivoVideo, 120)).Returns(preSignedUrl);
        _repositoryMock.Setup(r => r.SaveAsync(It.IsAny<ConversaoDb>(), cancellationToken)).Returns(Task.CompletedTask);
        _sqsServiceMock.Setup(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>())).ReturnsAsync(true);

        // Act
        var result = await _conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken);

        // Assert
        Assert.True(result);
        _s3ServiceMock.Verify(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo), Times.Once);
        _s3ServiceMock.Verify(s => s.GerarPreSignedUrl(urlArquivoVideo, 120), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversaoDb>(), cancellationToken), Times.Once);
        _sqsServiceMock.Verify(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Failure_PreSignedUrlIsEmpty()
    {
        // Arrange
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.AguardandoConversao, "video.mp4", new Mock<IFormFile>().Object);
        var cancellationToken = CancellationToken.None;
        var urlArquivoVideo = "https://amzn-s3-bucket-26bda3ac-c185-4185-a9f8-d3697a89754c-framepack.s3.amazonaws.com/aguardando-processamento/video.mp4";
        var preSignedUrl = string.Empty;

        _s3ServiceMock.Setup(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo)).ReturnsAsync(urlArquivoVideo);
        _s3ServiceMock.Setup(s => s.GerarPreSignedUrl(urlArquivoVideo, 120)).Returns(preSignedUrl);

        // Act
        var result = await _conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken);

        // Assert
        Assert.False(result);
        _s3ServiceMock.Verify(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo), Times.Once);
        _s3ServiceMock.Verify(s => s.GerarPreSignedUrl(urlArquivoVideo, 120), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversaoDb>(), cancellationToken), Times.Never);
        _sqsServiceMock.Verify(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>()), Times.Never);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Failure_UploadArquivoAsyncThrowsException()
    {
        // Arrange
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.AguardandoConversao, "video.mp4", new Mock<IFormFile>().Object);
        var cancellationToken = CancellationToken.None;

        _s3ServiceMock.Setup(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo)).ThrowsAsync(new AmazonS3Exception("Error uploading file"));

        // Act & Assert
        await Assert.ThrowsAsync<AmazonS3Exception>(() => _conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken));
        _s3ServiceMock.Verify(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo), Times.Once);
        _s3ServiceMock.Verify(s => s.GerarPreSignedUrl(It.IsAny<string>(), 120), Times.Never);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversaoDb>(), cancellationToken), Times.Never);
        _sqsServiceMock.Verify(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>()), Times.Never);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Failure_SendMessageAsyncReturnsFalse()
    {
        // Arrange
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, Status.AguardandoConversao, "video.mp4", new Mock<IFormFile>().Object);
        var cancellationToken = CancellationToken.None;
        var urlArquivoVideo = "https://amzn-s3-bucket-26bda3ac-c185-4185-a9f8-d3697a89754c-framepack.s3.amazonaws.com/aguardando-processamento/video.mp4";
        var preSignedUrl = "https://s3.amazonaws.com/bucket/video.mp4?signature=abc123";

        _s3ServiceMock.Setup(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo)).ReturnsAsync(urlArquivoVideo);
        _s3ServiceMock.Setup(s => s.GerarPreSignedUrl(urlArquivoVideo, 120)).Returns(preSignedUrl);
        _repositoryMock.Setup(r => r.SaveAsync(It.IsAny<ConversaoDb>(), cancellationToken)).Returns(Task.CompletedTask);
        _sqsServiceMock.Setup(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>())).ReturnsAsync(false);

        // Act
        var result = await _conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken);

        // Assert
        Assert.False(result);
        _s3ServiceMock.Verify(s => s.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo), Times.Once);
        _s3ServiceMock.Verify(s => s.GerarPreSignedUrl(urlArquivoVideo, 120), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<ConversaoDb>(), cancellationToken), Times.Once);
        _sqsServiceMock.Verify(s => s.SendMessageAsync(It.IsAny<ConversaoSolicitadaEvent>()), Times.Once);
    }
}
