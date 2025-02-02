using Domain.Entities;

namespace Framepack_WebApi.Tests.Core.Domain.Entities;

public class ArquivoTests
{
    [Fact]
    public void Arquivo_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var bytesArquivo = new byte[] { 1, 2, 3, 4 };
        var nomeArquivo = "teste.txt";

        // Act
        var arquivo = new Arquivo(bytesArquivo, nomeArquivo);

        // Assert
        Assert.Equal(bytesArquivo, arquivo.BytesArquivo);
        Assert.Equal(nomeArquivo, arquivo.NomeArquivo);
    }

    [Fact]
    public void Arquivo_Constructor_ShouldThrowArgumentNullException_WhenBytesArquivoIsNull()
    {
        // Arrange
        byte[] bytesArquivo = null;
        var nomeArquivo = "teste.txt";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Arquivo(bytesArquivo, nomeArquivo));
    }

    [Fact]
    public void Arquivo_Constructor_ShouldThrowArgumentNullException_WhenNomeArquivoIsNull()
    {
        // Arrange
        var bytesArquivo = new byte[] { 1, 2, 3, 4 };
        string nomeArquivo = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Arquivo(bytesArquivo, nomeArquivo));
    }

    [Fact]
    public void Arquivo_Constructor_ShouldThrowArgumentException_WhenNomeArquivoIsEmpty()
    {
        // Arrange
        var bytesArquivo = new byte[] { 1, 2, 3, 4 };
        var nomeArquivo = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Arquivo(bytesArquivo, nomeArquivo));
    }
}