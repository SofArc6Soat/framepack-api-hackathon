using Core.WebApi.Configurations;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Framepack_WebApi.Tests.Core.WebApi.Configurations;

public class UserContextServiceTests
{
    [Fact]
    public void UserId_ShouldReturnUserId_WhenUserIsAuthenticated()
    {
        // Arrange
        var userId = "12345";
        var claims = new List<Claim> { new("UserId", userId) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var userContextService = new UserContextService(httpContextAccessorMock.Object);

        // Act
        var result = userContextService.UserId;

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void UserId_ShouldReturnEmptyString_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var userContextService = new UserContextService(httpContextAccessorMock.Object);

        // Act
        var result = userContextService.UserId;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void UserId_ShouldReturnEmptyString_WhenHttpContextIsNull()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContextService = new UserContextService(httpContextAccessorMock.Object);

        // Act
        var result = userContextService.UserId;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void UserId_ShouldReturnEmptyString_WhenUserDoesNotHaveUserIdClaim()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var userContextService = new UserContextService(httpContextAccessorMock.Object);

        // Act
        var result = userContextService.UserId;

        // Assert
        Assert.Equal(string.Empty, result);
    }
}
