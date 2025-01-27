using Core.WebApi.GlobalErrorMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Framepack_WebApi.Tests.Core.WebApi.GlobalErrorMiddleware;

public class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void UseApplicationErrorMiddleware_ShouldAddMiddleware()
    {
        // Arrange
        var appBuilderMock = new Mock<IApplicationBuilder>();

        appBuilderMock.Setup(app => app.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                      .Returns(appBuilderMock.Object);

        // Act
        appBuilderMock.Object.UseApplicationErrorMiddleware();

        // Assert
        appBuilderMock.Verify(app => app.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.Once);
    }
}
