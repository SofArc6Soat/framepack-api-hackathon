using Core.Domain.DataAnnotations;

namespace Framepack_WebApi.Tests.Core.Domain.DataAnnotations;

public class RequiredGuidAttributeTests
{
    private readonly RequiredGuidAttribute _attribute;

    public RequiredGuidAttributeTests()
    {
        _attribute = new RequiredGuidAttribute();
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenValueIsNull()
    {
        // Arrange
        object value = null;

        // Act
        var result = _attribute.IsValid(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenValueIsNotGuid()
    {
        // Arrange
        object value = "not a guid";

        // Act
        var result = _attribute.IsValid(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenValueIsEmptyGuid()
    {
        // Arrange
        object value = Guid.Empty;

        // Act
        var result = _attribute.IsValid(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenValueIsValidGuid()
    {
        // Arrange
        object value = Guid.NewGuid();

        // Act
        var result = _attribute.IsValid(value);

        // Assert
        Assert.True(result);
    }
}