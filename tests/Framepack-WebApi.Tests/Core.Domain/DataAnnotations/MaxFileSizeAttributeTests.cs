using Core.Domain.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Framepack_WebApi.Tests.Core.Domain.DataAnnotations;

public class MaxFileSizeAttributeTests
{
    [Fact]
    public void IsValid_FileSizeWithinLimit_ReturnsSuccess()
    {
        // Arrange
        var maxFileSize = 1024; // 1 KB
        var attribute = new MaxFileSizeAttribute(maxFileSize);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(512); // 512 bytes

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void IsValid_FileSizeExceedsLimit_ReturnsValidationError()
    {
        // Arrange
        var maxFileSize = 1024; // 1 KB
        var attribute = new MaxFileSizeAttribute(maxFileSize);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(2048); // 2 KB

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal($"O tamanho do arquivo não deve exceder {maxFileSize} bytes.", result.ErrorMessage);
    }

    [Fact]
    public void IsValid_NullFile_ReturnsSuccess()
    {
        // Arrange
        var maxFileSize = 1024; // 1 KB
        var attribute = new MaxFileSizeAttribute(maxFileSize);

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void IsValid_FileSizeExceedsLimit_CustomErrorMessage_ReturnsValidationError()
    {
        // Arrange
        var maxFileSize = 1024; // 1 KB
        var attribute = new MaxFileSizeAttribute(maxFileSize)
        {
            ErrorMessage = "Arquivo muito grande."
        };
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(2048); // 2 KB

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal("Arquivo muito grande.", result.ErrorMessage);
    }
}