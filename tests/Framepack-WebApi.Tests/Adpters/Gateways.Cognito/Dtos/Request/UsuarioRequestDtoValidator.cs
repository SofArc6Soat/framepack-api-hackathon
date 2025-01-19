using FluentValidation;
using FluentValidation.TestHelper;
using Gateways.Cognito.Dtos.Request;

namespace Gateways.Cognito.Tests.Dtos.Request;

public class UsuarioRequestDtoValidator : AbstractValidator<UsuarioRequestDto>
{
    public UsuarioRequestDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("O campo {0} é obrigatório.");
        RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo {0} é obrigatório.")
                            .Length(2, 50).WithMessage("O campo {0} deve conter entre {2} e {1} caracteres.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("O campo {0} é obrigatório.")
                             .EmailAddress().WithMessage("{0} está em um formato inválido.")
                             .Length(5, 100).WithMessage("O campo {0} deve conter entre {2} e {1} caracteres.");
        RuleFor(x => x.Senha).NotEmpty().WithMessage("O campo {0} é obrigatório.")
                             .Length(8, 50).WithMessage("O campo {0} deve conter entre {2} e {1} caracteres.");
    }
}

public class UsuarioRequestDtoTests
{
    private readonly UsuarioRequestDtoValidator _validator;

    public UsuarioRequestDtoTests()
    {
        _validator = new UsuarioRequestDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new UsuarioRequestDto { Id = Guid.Empty, Nome = "Nome", Email = "email@domain.com", Senha = "Senha123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Is_Empty()
    {
        var model = new UsuarioRequestDto { Id = Guid.NewGuid(), Nome = "", Email = "email@domain.com", Senha = "Senha123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new UsuarioRequestDto { Id = Guid.NewGuid(), Nome = "Nome", Email = "invalid-email", Senha = "Senha123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Senha_Is_Too_Short()
    {
        var model = new UsuarioRequestDto { Id = Guid.NewGuid(), Nome = "Nome", Email = "email@domain.com", Senha = "123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Senha);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UsuarioRequestDto { Id = Guid.NewGuid(), Nome = "Nome", Email = "email@domain.com", Senha = "Senha123" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}