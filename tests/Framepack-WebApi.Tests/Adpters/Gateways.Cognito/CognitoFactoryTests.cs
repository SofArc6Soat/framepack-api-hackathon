using Gateways.Cognito;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito;

public class CognitoFactoryTests
{
    private readonly CognitoFactory _cognitoFactory;

    public CognitoFactoryTests()
    {
        _cognitoFactory = new CognitoFactory("test-client-id", "test-client-secret", "test-user-pool-id");
    }

    [Fact]
    public void CreateSignUpRequest_DeveRetornarSignUpRequestCorreto()
    {
        // Arrange
        var email = "usuario@teste.com";
        var senha = "senha123";
        var nome = "Usuario Teste";
        var cpf = "12345678901";

        // Act
        var result = _cognitoFactory.CreateSignUpRequest(email, senha, nome, cpf);

        // Assert
        Assert.Equal("test-client-id", result.ClientId);
        Assert.Equal(email, result.Username);
        Assert.Equal(senha, result.Password);
        Assert.Contains(result.UserAttributes, attr => attr.Name == "email" && attr.Value == email);
        Assert.Contains(result.UserAttributes, attr => attr.Name == "name" && attr.Value == nome);
        Assert.Contains(result.UserAttributes, attr => attr.Name == "custom:cpf" && attr.Value == cpf);
    }

    [Fact]
    public void CreateSignUpRequest_DeveRetornarSignUpRequestCorreto_SemCpf()
    {
        // Arrange
        var email = "usuario@teste.com";
        var senha = "senha123";
        var nome = "Usuario Teste";

        // Act
        var result = _cognitoFactory.CreateSignUpRequest(email, senha, nome);

        // Assert
        Assert.Equal("test-client-id", result.ClientId);
        Assert.Equal(email, result.Username);
        Assert.Equal(senha, result.Password);
        Assert.Contains(result.UserAttributes, attr => attr.Name == "email" && attr.Value == email);
        Assert.Contains(result.UserAttributes, attr => attr.Name == "name" && attr.Value == nome);
        Assert.DoesNotContain(result.UserAttributes, attr => attr.Name == "custom:cpf");
    }

    [Fact]
    public void CreateAddUserToGroupRequest_DeveRetornarAdminAddUserToGroupRequestCorreto()
    {
        // Arrange
        var email = "usuario@teste.com";
        var groupName = "grupo-teste";

        // Act
        var result = _cognitoFactory.CreateAddUserToGroupRequest(email, groupName);

        // Assert
        Assert.Equal("test-user-pool-id", result.UserPoolId);
        Assert.Equal(email, result.Username);
        Assert.Equal(groupName, result.GroupName);
    }

    [Fact]
    public void CreateConfirmSignUpRequest_DeveRetornarConfirmSignUpRequestCorreto()
    {
        // Arrange
        var email = "usuario@teste.com";
        var confirmationCode = "codigo123";

        // Act
        var result = _cognitoFactory.CreateConfirmSignUpRequest(email, confirmationCode);

        // Assert
        Assert.Equal("test-client-id", result.ClientId);
        Assert.Equal(email, result.Username);
        Assert.Equal(confirmationCode, result.ConfirmationCode);
    }

    [Fact]
    public void CreateForgotPasswordRequest_DeveRetornarForgotPasswordRequestCorreto()
    {
        // Arrange
        var email = "usuario@teste.com";

        // Act
        var result = _cognitoFactory.CreateForgotPasswordRequest(email);

        // Assert
        Assert.Equal("test-client-id", result.ClientId);
        Assert.Equal(email, result.Username);
    }

    [Fact]
    public void CreateConfirmForgotPasswordRequest_DeveRetornarConfirmForgotPasswordRequestCorreto()
    {
        // Arrange
        var email = "usuario@teste.com";
        var confirmationCode = "codigo123";
        var newPassword = "novaSenha123";

        // Act
        var result = _cognitoFactory.CreateConfirmForgotPasswordRequest(email, confirmationCode, newPassword);

        // Assert
        Assert.Equal("test-client-id", result.ClientId);
        Assert.Equal(email, result.Username);
        Assert.Equal(confirmationCode, result.ConfirmationCode);
        Assert.Equal(newPassword, result.Password);
    }

    [Fact]
    public void CreateListUsersRequestByEmail_DeveRetornarListUsersRequestCorreto()
    {
        // Arrange
        var email = "usuario@teste.com";

        // Act
        var result = _cognitoFactory.CreateListUsersRequestByEmail("test-user-pool-id", email);

        // Assert
        Assert.Equal("test-user-pool-id", result.UserPoolId);
        Assert.Equal($"email = \"{email}\"", result.Filter);
    }

    [Fact]
    public void CreateListUsersRequestByAll_DeveRetornarListUsersRequestCorreto()
    {
        // Arrange
        var paginationToken = "token123";

        // Act
        var result = _cognitoFactory.CreateListUsersRequestByAll("test-user-pool-id", paginationToken);

        // Assert
        Assert.Equal("test-user-pool-id", result.UserPoolId);
        Assert.Equal(paginationToken, result.PaginationToken);
    }

    [Fact]
    public void CreateAdminDeleteUserRequest_DeveRetornarAdminDeleteUserRequestCorreto()
    {
        // Arrange
        var username = "usuario@teste.com";

        // Act
        var result = _cognitoFactory.CreateAdminDeleteUserRequest("test-user-pool-id", username);

        // Assert
        Assert.Equal("test-user-pool-id", result.UserPoolId);
        Assert.Equal(username, result.Username);
    }

    [Fact]
    public void CreateInitiateSrpAuthRequest_DeveRetornarInitiateSrpAuthRequestCorreto()
    {
        // Arrange
        var password = "senha123";

        // Act
        var result = _cognitoFactory.CreateInitiateSrpAuthRequest(password);

        // Assert
        Assert.Equal(password, result.Password);
    }
}