using Moq;
using SmokeTests.FakeDataFactory;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

public class UsuarioControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public UsuarioControllerSmokeTest()
    {
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Post_CadastrarUsuarioEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarUsuarioValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios", request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_CadastrarUsuarioEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarUsuarioComEmailInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao cadastrar usuário\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao cadastrar usuário", content);
    }

    [Fact]
    public async Task Post_IdentificarUsuarioEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarIdentifiqueSeRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/identificar", request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_IdentificarUsuarioEndpoint_ReturnsUnauthorized()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarIdentifiqueSeRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/identificar", HttpStatusCode.Unauthorized, "{\"Success\":false, \"Errors\":[\"Usuário não autorizado\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/identificar", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Usuário não autorizado", content);
    }

    [Fact]
    public async Task Post_ConfirmarEmailVerificacaoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarConfirmarEmailVerificacaoDtoValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/confirmar-email", request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_ConfirmarEmailVerificacaoEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarConfirmarEmailVerificacaoDtoInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/confirmar-email", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao confirmar e-mail\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/confirmar-email", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao confirmar e-mail", content);
    }

    [Fact]
    public async Task Post_SolicitarRecuperacaoSenhaEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarSolicitarRecuperacaoSenhaDtoValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/recuperar-senha", request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_SolicitarRecuperacaoSenhaEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarSolicitarRecuperacaoSenhaDtoInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/recuperar-senha", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao solicitar recuperação de senha\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/recuperar-senha", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao solicitar recuperação de senha", content);
    }

    [Fact]
    public async Task Post_EfetuarResetSenhaEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarResetarSenhaDtoValido();

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/resetar-senha", request);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_EfetuarResetSenhaEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = UsuarioFakeDataFactory.CriarResetarSenhaDtoInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/usuarios/resetar-senha", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao resetar senha\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/usuarios/resetar-senha", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao resetar senha", content);
    }
}