using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Configurations;
using Moq;
using System.Net;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito;

public class CognitoGatewayTests
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _mockCognitoClient;
    private readonly Mock<ICognitoFactory> _mockCognitoFactory;
    private readonly Mock<ICognitoConfig> _mockCognitoConfig;
    private readonly CognitoGateway _cognitoGateway;

    public CognitoGatewayTests()
    {
        _mockCognitoClient = new Mock<IAmazonCognitoIdentityProvider>();
        _mockCognitoFactory = new Mock<ICognitoFactory>();
        _mockCognitoConfig = new Mock<ICognitoConfig>();

        _mockCognitoConfig.SetupGet(x => x.ClientId).Returns("test-client-id");
        _mockCognitoConfig.SetupGet(x => x.ClientSecret).Returns("test-client-secret");
        _mockCognitoConfig.SetupGet(x => x.UserPoolId).Returns("us-east-1_test-user-pool-id");

        _cognitoGateway = new CognitoGateway(_mockCognitoClient.Object, _mockCognitoFactory.Object, _mockCognitoConfig.Object);
    }

    [Fact]
    public async Task CriarUsuarioAsync_Success()
    {
        var usuario = new Usuario(Guid.NewGuid(), "Test User", "test@example.com");
        var senha = "Test@123";
        var signUpRequest = new SignUpRequest();
        var addToGroupRequest = new AdminAddUserToGroupRequest();
        var listUsersRequest = new ListUsersRequest();
        var listUsersResponse = new ListUsersResponse { Users = new List<UserType>() };

        _mockCognitoFactory.Setup(x => x.CreateSignUpRequest(usuario.Email, senha, usuario.Nome, null)).Returns(signUpRequest);
        _mockCognitoFactory.Setup(x => x.CreateAddUserToGroupRequest(usuario.Email, "admin")).Returns(addToGroupRequest);
        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), usuario.Email)).Returns(listUsersRequest);
        _mockCognitoClient.Setup(x => x.ListUsersAsync(listUsersRequest, It.IsAny<CancellationToken>())).ReturnsAsync(listUsersResponse);
        _mockCognitoClient.Setup(x => x.SignUpAsync(signUpRequest, It.IsAny<CancellationToken>())).ReturnsAsync(new SignUpResponse { HttpStatusCode = HttpStatusCode.OK });
        _mockCognitoClient.Setup(x => x.AdminAddUserToGroupAsync(addToGroupRequest, It.IsAny<CancellationToken>())).ReturnsAsync(new AdminAddUserToGroupResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.CriarUsuarioAsync(usuario, senha, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task CriarUsuarioAsync_EmailExists()
    {
        var usuario = new Usuario(Guid.NewGuid(), "Test User", "test@example.com");
        var senha = "Test@123";
        var listUsersRequest = new ListUsersRequest();
        var listUsersResponse = new ListUsersResponse { Users = new List<UserType> { new UserType() } };

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), usuario.Email)).Returns(listUsersRequest);
        _mockCognitoClient.Setup(x => x.ListUsersAsync(listUsersRequest, It.IsAny<CancellationToken>())).ReturnsAsync(listUsersResponse);

        var result = await _cognitoGateway.CriarUsuarioAsync(usuario, senha, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_Success()
    {
        var emailVerificacao = new EmailVerificacao("test@example.com", "123456");
        var confirmSignUpRequest = new ConfirmSignUpRequest();

        _mockCognitoFactory.Setup(x => x.CreateConfirmSignUpRequest(emailVerificacao.Email, emailVerificacao.CodigoVerificacao)).Returns(confirmSignUpRequest);
        _mockCognitoClient.Setup(x => x.ConfirmSignUpAsync(confirmSignUpRequest, It.IsAny<CancellationToken>())).ReturnsAsync(new ConfirmSignUpResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_Failure()
    {
        var emailVerificacao = new EmailVerificacao("test@example.com", "123456");
        var confirmSignUpRequest = new ConfirmSignUpRequest();

        _mockCognitoFactory.Setup(x => x.CreateConfirmSignUpRequest(emailVerificacao.Email, emailVerificacao.CodigoVerificacao)).Returns(confirmSignUpRequest);
        _mockCognitoClient.Setup(x => x.ConfirmSignUpAsync(confirmSignUpRequest, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Success()
    {
        var recuperacaoSenha = new RecuperacaoSenha("test@example.com");
        var forgotPasswordRequest = new ForgotPasswordRequest();

        _mockCognitoFactory.Setup(x => x.CreateForgotPasswordRequest(recuperacaoSenha.Email)).Returns(forgotPasswordRequest);
        _mockCognitoClient.Setup(x => x.ForgotPasswordAsync(forgotPasswordRequest, It.IsAny<CancellationToken>())).ReturnsAsync(new ForgotPasswordResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Failure()
    {
        var recuperacaoSenha = new RecuperacaoSenha("test@example.com");
        var forgotPasswordRequest = new ForgotPasswordRequest();

        _mockCognitoFactory.Setup(x => x.CreateForgotPasswordRequest(recuperacaoSenha.Email)).Returns(forgotPasswordRequest);
        _mockCognitoClient.Setup(x => x.ForgotPasswordAsync(forgotPasswordRequest, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Success()
    {
        var resetSenha = new ResetSenha("test@example.com", "123456", "NewPassword@123");
        var confirmForgotPasswordRequest = new ConfirmForgotPasswordRequest();

        _mockCognitoFactory.Setup(x => x.CreateConfirmForgotPasswordRequest(resetSenha.Email, resetSenha.CodigoVerificacao, resetSenha.NovaSenha)).Returns(confirmForgotPasswordRequest);
        _mockCognitoClient.Setup(x => x.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest, It.IsAny<CancellationToken>())).ReturnsAsync(new ConfirmForgotPasswordResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Failure()
    {
        var resetSenha = new ResetSenha("test@example.com", "123456", "NewPassword@123");
        var confirmForgotPasswordRequest = new ConfirmForgotPasswordRequest();

        _mockCognitoFactory.Setup(x => x.CreateConfirmForgotPasswordRequest(resetSenha.Email, resetSenha.CodigoVerificacao, resetSenha.NovaSenha)).Returns(confirmForgotPasswordRequest);
        _mockCognitoClient.Setup(x => x.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_Success()
    {
        var email = "test@example.com";
        var username = "test-username";
        var deleteUserRequest = new AdminDeleteUserRequest();

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), email)).Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType { Username = username } } });
        _mockCognitoFactory.Setup(x => x.CreateAdminDeleteUserRequest(It.IsAny<string>(), username)).Returns(deleteUserRequest);
        _mockCognitoClient.Setup(x => x.AdminDeleteUserAsync(deleteUserRequest, It.IsAny<CancellationToken>())).ReturnsAsync(new AdminDeleteUserResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_UserNotFound()
    {
        var email = "test@example.com";

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), email)).Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task IdentifiqueSeAsync_InvalidCredentials()
    {
        var email = "test@example.com";
        var senha = "Test@123";

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), email)).Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType { Username = "test-user" } } });
        _mockCognitoFactory.Setup(x => x.CreateInitiateSrpAuthRequest(senha)).Returns(new InitiateSrpAuthRequest());
        _mockCognitoClient.Setup(x => x.AdminInitiateAuthAsync(It.IsAny<AdminInitiateAuthRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotAuthorizedException("Invalid credentials"));

        _mockCognitoFactory.Setup(x => x.CreateInitiateSrpAuthRequest(It.IsAny<string>())).Throws(new NotAuthorizedException("Invalid credentials"));

        await Assert.ThrowsAsync<NotAuthorizedException>(() => _cognitoGateway.IdentifiqueSeAsync(email, senha, CancellationToken.None));
    }
}