using FluentValidation;
using FluentValidation.TestHelper;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Gateways.Tests.Dtos.Request;

public class UploadRequestDtoValidator : AbstractValidator<UploadRequestDto>
{
    public UploadRequestDtoValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("O campo {0} é obrigatório.");
        RuleFor(x => x.NomeArquivo)
            .NotEmpty().WithMessage("O campo {0} é obrigatório.")
            .Length(5, 50).WithMessage("O campo {0} deve conter entre {2} e {1} caracteres.");
        RuleFor(x => x.ArquivoVideo)
            .NotNull().WithMessage("O {0} é obrigatório.")
            .Must(HaveValidExtension).WithMessage("Extensão de arquivo inválida.")
            .Must(HaveValidSize).WithMessage("O tamanho do arquivo excede o limite permitido.");
    }

    private bool HaveValidExtension(IFormFile file)
    {
        if (file == null) return false;
        var allowedExtensions = new List<string> { ".mp4" };
        var extension = System.IO.Path.GetExtension(file.FileName);
        return allowedExtensions.Contains(extension.ToLower());
    }

    private bool HaveValidSize(IFormFile file)
    {
        if (file == null) return false;
        const long maxFileSize = 500 * 1024 * 1024; // 500 MB
        return file.Length <= maxFileSize;
    }
}

public class UploadRequestDtoTests
{
    private readonly UploadRequestDtoValidator _validator;

    public UploadRequestDtoTests()
    {
        _validator = new UploadRequestDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UsuarioId_Is_Empty()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.Empty,
            NomeArquivo = "validname.mp4",
            ArquivoVideo = CreateMockFile("video.mp4", 100 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.UsuarioId);
    }

    [Fact]
    public void Should_Have_Error_When_NomeArquivo_Is_Empty()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = string.Empty,
            ArquivoVideo = CreateMockFile("video.mp4", 100 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NomeArquivo);
    }

    [Fact]
    public void Should_Have_Error_When_NomeArquivo_Is_Too_Short()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "abc",
            ArquivoVideo = CreateMockFile("video.mp4", 100 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NomeArquivo);
    }

    [Fact]
    public void Should_Have_Error_When_NomeArquivo_Is_Too_Long()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = new string('a', 51),
            ArquivoVideo = CreateMockFile("video.mp4", 100 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NomeArquivo);
    }

    [Fact]
    public void Should_Have_Error_When_ArquivoVideo_Is_Null()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "validname.mp4",
            ArquivoVideo = null
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ArquivoVideo);
    }

    [Fact]
    public void Should_Have_Error_When_ArquivoVideo_Has_Invalid_Extension()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "validname.mp4",
            ArquivoVideo = CreateMockFile("video.txt", 100 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ArquivoVideo);
    }

    [Fact]
    public void Should_Have_Error_When_ArquivoVideo_Exceeds_Max_Size()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "validname.mp4",
            ArquivoVideo = CreateMockFile("video.mp4", 600 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ArquivoVideo);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Dto_Is_Valid()
    {
        var dto = new UploadRequestDto
        {
            UsuarioId = Guid.NewGuid(),
            NomeArquivo = "validname.mp4",
            ArquivoVideo = CreateMockFile("video.mp4", 100 * 1024 * 1024)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private IFormFile CreateMockFile(string fileName, long length)
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(length);
        return mockFile.Object;
    }
}
