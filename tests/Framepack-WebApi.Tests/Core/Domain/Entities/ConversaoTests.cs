using Domain.Entities;
using Domain.ValueObjects;

namespace Framepack_WebApi.Tests.Core.Domain.Entities;

public class ConversaoTests
{
    [Fact]
    public void Conversao_DeveSerCriadaComValoresValidos()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var data = DateTime.Now;
        var status = Status.Aguardando;
        var nomeArquivo = "video.mp4";
        var urlArquivoVideo = "http://example.com/video.mp4";
        var urlArquivoCompactado = "http://example.com/video.zip";

        // Act
        var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, urlArquivoVideo, urlArquivoCompactado);

        // Assert
        Assert.Equal(id, conversao.Id);
        Assert.Equal(usuarioId, conversao.UsuarioId);
        Assert.Equal(data, conversao.Data);
        Assert.Equal(status, conversao.Status);
        Assert.Equal(nomeArquivo, conversao.NomeArquivo);
        Assert.Equal(urlArquivoVideo, conversao.UrlArquivoVideo);
        Assert.Equal(urlArquivoCompactado, conversao.UrlArquivoCompactado);
    }

    [Fact]
    public void Conversao_DevePermitirUrlsNulas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var data = DateTime.Now;
        var status = Status.Aguardando;
        var nomeArquivo = "video.mp4";

        // Act
        var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, null, null);

        // Assert
        Assert.Null(conversao.UrlArquivoVideo);
        Assert.Null(conversao.UrlArquivoCompactado);
    }

    [Fact]
    public void ValidarConversao_DeveValidarComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var data = DateTime.Now;
        var status = Status.Aguardando;
        var nomeArquivo = "video.mp4";
        var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, null, null);
        var validator = new ValidarConversao();

        // Act
        var result = validator.Validate(conversao);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidarConversao_DeveFalharQuandoIdForInvalido()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var data = DateTime.Now;
        var status = Status.Aguardando;
        var nomeArquivo = "video.mp4";
        var conversao = new Conversao(Guid.Empty, usuarioId, data, status, nomeArquivo, null, null);
        var validator = new ValidarConversao();

        // Act
        var result = validator.Validate(conversao);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Id");
    }

    [Fact]
    public void ValidarConversao_DeveFalharQuandoUsuarioIdForInvalido()
    {
        // Arrange
        var id = Guid.NewGuid();
        var data = DateTime.Now;
        var status = Status.Aguardando;
        var nomeArquivo = "video.mp4";
        var conversao = new Conversao(id, Guid.Empty, data, status, nomeArquivo, null, null);
        var validator = new ValidarConversao();

        // Act
        var result = validator.Validate(conversao);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UsuarioId");
    }

    [Fact]
    public void ValidarConversao_DeveFalharQuandoDataForInvalida()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var status = Status.Aguardando;
        var nomeArquivo = "video.mp4";
        var conversao = new Conversao(id, usuarioId, default, status, nomeArquivo, null, null);
        var validator = new ValidarConversao();

        // Act
        var result = validator.Validate(conversao);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Data");
    }

    [Fact]
    public void ValidarConversao_DeveFalharQuandoNomeArquivoForInvalido()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var data = DateTime.Now;
        var status = Status.Aguardando;
        var nomeArquivo = "a"; // NomeArquivo com menos de 2 caracteres
        var conversao = new Conversao(id, usuarioId, data, status, nomeArquivo, null, null);
        var validator = new ValidarConversao();

        // Act
        var result = validator.Validate(conversao);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "NomeArquivo");
    }
}
