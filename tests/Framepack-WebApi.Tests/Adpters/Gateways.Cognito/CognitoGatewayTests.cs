using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
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
        _mockCognitoConfig.SetupGet(x => x.UserPoolId).Returns("us-east-1_test-pool-id");

        _cognitoGateway = new CognitoGateway(_mockCognitoClient.Object, _mockCognitoFactory.Object, _mockCognitoConfig.Object);
    }

    [Fact]
    public async Task CriarUsuarioAsync_Success()
    {
        var usuario = new Usuario(Guid.NewGuid(), "Test User", "test@example.com");
        var senha = "Test@123";
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateSignUpRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
            .Returns(new SignUpRequest());
        _mockCognitoFactory.Setup(x => x.CreateAddUserToGroupRequest(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new AdminAddUserToGroupRequest());

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        _mockCognitoClient.Setup(x => x.SignUpAsync(It.IsAny<SignUpRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SignUpResponse { HttpStatusCode = HttpStatusCode.OK });
        _mockCognitoClient.Setup(x => x.AdminAddUserToGroupAsync(It.IsAny<AdminAddUserToGroupRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdminAddUserToGroupResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.CriarUsuarioAsync(usuario, senha, cancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task CriarUsuarioAsync_EmailExists()
    {
        var usuario = new Usuario(Guid.NewGuid(), "Test User", "test@example.com");
        var senha = "Test@123";
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType() } });

        var result = await _cognitoGateway.CriarUsuarioAsync(usuario, senha, cancellationToken);

        Assert.False(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_Success()
    {
        var emailVerificacao = new EmailVerificacao("test@example.com", "123456");
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateConfirmSignUpRequest(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ConfirmSignUpRequest());
        _mockCognitoClient.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfirmSignUpResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task ConfirmarEmailVerificacaoAsync_Failure()
    {
        var emailVerificacao = new EmailVerificacao("test@example.com", "123456");
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateConfirmSignUpRequest(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ConfirmSignUpRequest());
        _mockCognitoClient.Setup(x => x.ConfirmSignUpAsync(It.IsAny<ConfirmSignUpRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var result = await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken);

        Assert.False(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Success()
    {
        var recuperacaoSenha = new RecuperacaoSenha("test@example.com");
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateForgotPasswordRequest(It.IsAny<string>()))
            .Returns(new ForgotPasswordRequest());
        _mockCognitoClient.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ForgotPasswordResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Failure()
    {
        var recuperacaoSenha = new RecuperacaoSenha("test@example.com");
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateForgotPasswordRequest(It.IsAny<string>()))
            .Returns(new ForgotPasswordRequest());
        _mockCognitoClient.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var result = await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken);

        Assert.False(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Success()
    {
        var resetSenha = new ResetSenha("test@example.com", "123456", "NewPassword@123");
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateConfirmForgotPasswordRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ConfirmForgotPasswordRequest());
        _mockCognitoClient.Setup(x => x.ConfirmForgotPasswordAsync(It.IsAny<ConfirmForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConfirmForgotPasswordResponse { HttpStatusCode = HttpStatusCode.OK });

        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, cancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task EfetuarResetSenhaAsync_Failure()
    {
        var resetSenha = new ResetSenha("test@example.com", "123456", "NewPassword@123");
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateConfirmForgotPasswordRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ConfirmForgotPasswordRequest());
        _mockCognitoClient.Setup(x => x.ConfirmForgotPasswordAsync(It.IsAny<ConfirmForgotPasswordRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var result = await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, cancellationToken);

        Assert.False(result);
    }

    [Fact]
    public async Task IdentifiqueSeAsync_UserNotFound()
    {
        var email = "test@example.com";
        var senha = "Test@123";
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        var result = await _cognitoGateway.IdentifiqueSeAsync(email, senha, cancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_Success()
    {
        var email = "test@example.com";
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType { Username = "test-user-id" } } });

        _mockCognitoFactory.Setup(x => x.CreateAdminDeleteUserRequest(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new AdminDeleteUserRequest());
        _mockCognitoClient.Setup(x => x.AdminDeleteUserAsync(It.IsAny<AdminDeleteUserRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new AdminDeleteUserResponse()));

        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, cancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task DeletarUsuarioCognitoAsync_UserNotFound()
    {
        var email = "test@example.com";
        var cancellationToken = CancellationToken.None;

        _mockCognitoFactory.Setup(x => x.CreateListUsersRequestByEmail(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new ListUsersRequest());
        _mockCognitoClient.Setup(x => x.ListUsersAsync(It.IsAny<ListUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse { Users = new List<UserType>() });

        var result = await _cognitoGateway.DeletarUsuarioCognitoAsync(email, cancellationToken);

        Assert.False(result);
    }
}