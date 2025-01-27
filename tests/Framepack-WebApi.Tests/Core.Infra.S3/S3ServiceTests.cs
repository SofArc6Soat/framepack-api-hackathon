using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Core.Infra.S3;
using Moq;
using System.Net;

namespace Framepack_WebApi.Tests.Core.Infra.S3;

public class S3ServiceTests
{
    private readonly Mock<IAmazonS3> _s3ClientMock;
    private readonly Mock<TransferUtility> _transferUtilityMock;
    private readonly S3Service _s3Service;

    public S3ServiceTests()
    {
        _s3ClientMock = new Mock<IAmazonS3>();
        _transferUtilityMock = new Mock<TransferUtility>(_s3ClientMock.Object);
        _s3Service = new S3Service(_s3ClientMock.Object);
    }

    [Fact]
    public void GerarPreSignedUrl_Success()
    {
        // Arrange
        var key = "some/key";
        var url = "http://example.com";

        _s3ClientMock.Setup(x => x.GetPreSignedURL(It.IsAny<GetPreSignedUrlRequest>())).Returns(url);

        // Act
        var result = _s3Service.GerarPreSignedUrl(key);

        // Assert
        Assert.Equal(url, result);
    }

    [Fact]
    public async Task DeletarArquivoAsync_Success()
    {
        // Arrange
        var key = "some/key";

        _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default))
            .ReturnsAsync(new DeleteObjectResponse());

        // Act
        await _s3Service.DeletarArquivoAsync(key);

        // Assert
        _s3ClientMock.Verify(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default), Times.Once);
    }

    [Fact]
    public async Task DeletarArquivoAsync_Failure()
    {
        // Arrange
        var key = "some/key";

        _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default))
            .ThrowsAsync(new AmazonS3Exception("Error deleting file"));

        // Act & Assert
        await Assert.ThrowsAsync<AmazonS3Exception>(() => _s3Service.DeletarArquivoAsync(key));
    }

    [Fact]
    public async Task VerificarExistenciaArquivo_Success()
    {
        // Arrange
        var bucketName = "bucket";
        var key = "some/key";

        _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(bucketName, key, default))
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
        var key = "some/key";

        _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(bucketName, key, default))
            .ThrowsAsync(new AmazonS3Exception("File not found") { StatusCode = HttpStatusCode.NotFound });

        // Act
        var result = await _s3Service.VerificarExistenciaArquivo(bucketName, key);

        // Assert
        Assert.False(result);
    }
}