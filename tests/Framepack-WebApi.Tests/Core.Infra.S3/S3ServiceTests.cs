using Amazon.S3;
using Amazon.S3.Model;
using Core.Infra.S3;
using Moq;
using System.Net;

namespace Framepack_WebApi.Tests.Core.Infra.S3;

public class S3ServiceTests
{
    private readonly Mock<IAmazonS3> _mockS3Client;
    private readonly S3Service _s3Service;

    public S3ServiceTests()
    {
        _mockS3Client = new Mock<IAmazonS3>();
        _s3Service = new S3Service(_mockS3Client.Object);
    }

    [Fact]
    public void GerarPreSignedUrl_Success()
    {
        // Arrange
        var key = "test-key";
        var url = "https://bucket.s3.amazonaws.com/test-key";

        _mockS3Client.Setup(s => s.GetPreSignedURL(It.IsAny<GetPreSignedUrlRequest>())).Returns(url);

        // Act
        var result = _s3Service.GerarPreSignedUrl(key);

        // Assert
        Assert.Equal(url, result);
    }

    [Fact]
    public async Task DeletarArquivoAsync_Success()
    {
        // Arrange
        var key = "test-key";

        _mockS3Client.Setup(s => s.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default))
            .ReturnsAsync(new DeleteObjectResponse());

        // Act
        await _s3Service.DeletarArquivoAsync(key);

        // Assert
        _mockS3Client.Verify(s => s.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default), Times.Once);
    }

    [Fact]
    public async Task DeletarArquivoAsync_Failure()
    {
        // Arrange
        var key = "test-key";

        _mockS3Client.Setup(s => s.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default))
            .ThrowsAsync(new AmazonS3Exception("Erro ao deletar o arquivo"));

        // Act & Assert
        await Assert.ThrowsAsync<AmazonS3Exception>(() => _s3Service.DeletarArquivoAsync(key));
    }

    [Fact]
    public async Task VerificarExistenciaArquivo_Success()
    {
        // Arrange
        var bucketName = "bucket";
        var key = "test-key";

        _mockS3Client.Setup(s => s.GetObjectMetadataAsync(bucketName, key, default))
            .ReturnsAsync(new GetObjectMetadataResponse { HttpStatusCode = HttpStatusCode.OK });

        // Act
        var result = await _s3Service.VerificarExistenciaArquivo(bucketName, key);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerificarExistenciaArquivo_NotFound()
    {
        // Arrange
        var bucketName = "bucket";
        var key = "test-key";

        _mockS3Client.Setup(s => s.GetObjectMetadataAsync(bucketName, key, default))
            .ThrowsAsync(new AmazonS3Exception("Not Found") { StatusCode = HttpStatusCode.NotFound });

        // Act
        var result = await _s3Service.VerificarExistenciaArquivo(bucketName, key);

        // Assert
        Assert.False(result);
    }
}