using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Framepack_WebApi.Tests.Core.WebApi.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsTests() => _services = new ServiceCollection();

    [Fact]
    public void AddApiDefautConfig_ShouldConfigureAllServices_Successfully()
    {
        // Arrange
        var jwtOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        _services.AddApiDefautConfig(jwtOptions);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<INotificador>());
        Assert.NotNull(serviceProvider.GetService<IOptions<JsonOptions>>());
        Assert.NotNull(serviceProvider.GetService<IOptions<TokenValidationParameters>>());
    }

    [Fact]
    public void AddApiDefautConfig_ShouldThrowArgumentNullException_WhenJwtOptionsIsNull()
    {
        // Arrange
        JwtBearerConfigureOptions jwtOptions = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _services.AddApiDefautConfig(jwtOptions));
    }

    [Fact]
    public void AddApiDefautConfig_ShouldAddAuthenticationAndAuthorization()
    {
        // Arrange
        var jwtOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        _services.AddApiDefautConfig(jwtOptions);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var authService = serviceProvider.GetService<IAuthenticationService>();
        Assert.NotNull(authService);
    }
}