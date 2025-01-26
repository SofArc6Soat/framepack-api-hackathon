using Core.Domain.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Framepack_WebApi.Tests.Core.Domain.DataAnnotations;

public class AllowedExtensionsAttributeTests
{
    [Fact]
    public void IsValid_FileWithAllowedExtension_ReturnsSuccess()
    {
        // Arrange
        var allowedExtensions = new[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("image.jpg");

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void IsValid_FileWithDisallowedExtension_ReturnsValidationError()
    {
        // Arrange
        var allowedExtensions = new[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("document.pdf");

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal($"O arquivo deve ter uma das seguintes extensões: {string.Join(", ", allowedExtensions)}.", result.ErrorMessage);
    }

    [Fact]
    public void IsValid_NullFile_ReturnsSuccess()
    {
        // Arrange
        var allowedExtensions = new[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void IsValid_FileWithUpperCaseExtension_ReturnsSuccess()
    {
        // Arrange
        var allowedExtensions = new[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("image.JPG");

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void IsValid_FileWithNoExtension_ReturnsValidationError()
    {
        // Arrange
        var allowedExtensions = new[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("image");

        // Act
        var result = attribute.GetValidationResult(fileMock.Object, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal($"O arquivo deve ter uma das seguintes extensões: {string.Join(", ", allowedExtensions)}.", result.ErrorMessage);
    }
}