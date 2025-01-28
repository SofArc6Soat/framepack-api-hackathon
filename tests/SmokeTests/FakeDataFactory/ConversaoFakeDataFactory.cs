using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Http;
using Moq;

namespace SmokeTests.FakeDataFactory;

public static class ConversaoFakeDataFactory
{
    public static UploadRequestDto CriarUploadRequestValido()
    {
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.FileName).Returns("video.mp4");
        formFile.Setup(f => f.Length).Returns(1024 * 1024 * 10); // 10 MB

        return new UploadRequestDto
        {
            NomeArquivo = "video.mp4",
            ArquivoVideo = formFile.Object
        };
    }

    public static UploadRequestDto CriarUploadRequestInvalido()
    {
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.FileName).Returns("video.txt");
        formFile.Setup(f => f.Length).Returns(1024 * 1024 * 10); // 10 MB

        return new UploadRequestDto
        {
            NomeArquivo = "video.txt",
            ArquivoVideo = formFile.Object
        };
    }
}