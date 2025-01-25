using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Domain.Entities;
using Gateways.Cognito;
using Gateways.Cognito.Configurations;
using Moq;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito;

public class CognitoGatewayTests
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _mockCognitoClient;
    private readonly Mock<ICognitoFactory> _mockFactory;
    private readonly Mock<ICognitoConfig> _mockConfig;
    private readonly CognitoGateway _cognitoGateway;

    public CognitoGatewayTests()
    {
        _mockCognitoClient = new Mock<IAmazonCognitoIdentityProvider>();
        _mockFactory = new Mock<ICognitoFactory>();
        _mockConfig = new Mock<ICognitoConfig>();

        _mockConfig.SetupGet(x => x.ClientId).Returns("test-client-id");
        _mockConfig.SetupGet(x => x.ClientSecret).Returns("test-client-secret");
        _mockConfig.SetupGet(x => x.UserPoolId).Returns("test-user-pool-id");

        _cognitoGateway = new CognitoGateway(
            _mockCognitoClient.Object,
            _mockFactory.Object,
            _mockConfig.Object);
    }

    [Fact]
    public async Task CriarUsuarioAsync_EmailJaExiste_DeveRetornarFalse()
    {
        // Arrange
        var usuario = new Usuario(Guid.NewGuid(), "Test User", "test@example.com");
        _mockFactory.Setup(f => f.CreateListUsersRequestByEmail(It.IsAny<string>(), usuario.Email))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(c => c.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = [new UserType()] });

        // Act
        var result = await _cognitoGateway.CriarUsuarioAsync(usuario, "password", CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CriarUsuarioAsync_EmailNaoExiste_DeveCriarUsuario()
    {
        // Arrange
        var usuario = new Usuario(Guid.NewGuid(), "Test User", "test@example.com");
        var signUpRequest = new SignUpRequest();
        var addToGroupRequest = new AdminAddUserToGroupRequest();

        // Configurações de fábrica
        _mockFactory.Setup(f => f.CreateSignUpRequest(usuario.Email, "password", usuario.Nome, null))
            .Returns(signUpRequest);
        _mockFactory.Setup(f => f.CreateAddUserToGroupRequest(usuario.Email, "admin"))
            .Returns(addToGroupRequest);

        // Configuração para e-mail inexistente
        _mockFactory.Setup(f => f.CreateListUsersRequestByEmail(It.IsAny<string>(), usuario.Email))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(c => c.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = [] });

        // Configuração para criação de usuário
        _mockCognitoClient.Setup(c => c.SignUpAsync(signUpRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Configuração para adicionar usuário ao grupo
        _mockCognitoClient.Setup(c => c.AdminAddUserToGroupAsync(addToGroupRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdminAddUserToGroupResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });

        // Act
        var result = await _cognitoGateway.CriarUsuarioAsync(usuario, "password", CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}