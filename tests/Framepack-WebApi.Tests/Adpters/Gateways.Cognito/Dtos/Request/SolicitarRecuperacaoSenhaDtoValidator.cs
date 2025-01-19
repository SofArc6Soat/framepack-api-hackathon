using FluentValidation;
using FluentValidation.TestHelper;
using Gateways.Cognito.Dtos.Request;

namespace Gateways.Cognito.Tests.Dtos.Request;

public class SolicitarRecuperacaoSenhaDtoValidator : AbstractValidator<SolicitarRecuperacaoSenhaDto>
{
    public SolicitarRecuperacaoSenhaDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("O campo {0} é obrigatório.")
                             .EmailAddress().WithMessage("{0} está em um formato inválido.")
                             .Length(5, 100).WithMessage("O campo {0} deve conter entre {2} e {1} caracteres.");
    }
}

public class SolicitarRecuperacaoSenhaDtoTests
{
    private readonly SolicitarRecuperacaoSenhaDtoValidator _validator;

    public SolicitarRecuperacaoSenhaDtoTests()
    {
        _validator = new SolicitarRecuperacaoSenhaDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Too_Short()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "a@b.c" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var model = new SolicitarRecuperacaoSenhaDto { Email = "email@domain.com" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}