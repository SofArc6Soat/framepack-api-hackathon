using Core.WebApi.GlobalErrorMiddleware;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Framepack_WebApi.Tests.Core.WebApi.GlobalErrorMiddleware;

public class ApplicationErrorMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldCallNextDelegate_WhenNoExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        mockRequestDelegate.Setup(rd => rd(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

        var middleware = new ApplicationErrorMiddleware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        mockRequestDelegate.Verify(rd => rd(It.IsAny<HttpContext>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn500StatusCode_WhenExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        mockRequestDelegate.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(new Exception("Test exception"));

        var middleware = new ApplicationErrorMiddleware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(500, context.Response.StatusCode);
        mockRequestDelegate.Verify(rd => rd(It.IsAny<HttpContext>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnErrorMessage_WhenExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new System.IO.MemoryStream();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        mockRequestDelegate.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(new Exception("Test exception"));

        var middleware = new ApplicationErrorMiddleware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
        var reader = new System.IO.StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        Assert.Equal("An unexpected fault happened. Try again later.", responseBody);
    }
}
