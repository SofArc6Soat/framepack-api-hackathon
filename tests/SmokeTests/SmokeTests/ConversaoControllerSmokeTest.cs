using Moq;
using SmokeTests.FakeDataFactory;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

public class ConversaoControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public ConversaoControllerSmokeTest()
    {
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Post_EfetuarUploadEndpoint_ReturnsSuccess()
    {
        // Arrange
        var request = ConversaoFakeDataFactory.CriarUploadRequestValido();

        // Act
        var response = await _client.PostAsJsonAsync("/conversao/upload", request);

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
    public async Task Post_EfetuarUploadEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var request = ConversaoFakeDataFactory.CriarUploadRequestInvalido();
        _handlerMock.SetupRequest(HttpMethod.Post, "http://localhost/conversao/upload", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao efetuar upload\"]}");

        // Act
        var response = await _client.PostAsJsonAsync("/conversao/upload", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao efetuar upload", content);
    }
}