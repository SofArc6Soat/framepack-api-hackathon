using Core.Domain.Entities;

namespace Framepack_WebApi.Tests.Core.Domain.Entities;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_DefaultConstructor_ShouldCreateInstance()
    {
        // Act
        var exception = new DomainException();

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Exception of type 'Core.Domain.Entities.DomainException' was thrown.", exception.Message);
    }

    [Fact]
    public void DomainException_MessageConstructor_ShouldSetMessage()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void DomainException_MessageAndInnerExceptionConstructor_ShouldSetMessageAndInnerException()
    {
        // Arrange
        var message = "Test message";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new DomainException(message, innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
