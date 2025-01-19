using FluentValidation;
using Gateways.Cognito.Dtos.Request;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito
{
    public class SolicitarRecuperacaoSenhaDtoValidator : AbstractValidator<SolicitarRecuperacaoSenhaDto>
    {
        public SolicitarRecuperacaoSenhaDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O campo E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail está em um formato inválido.")
                .Length(5, 100).WithMessage("O campo E-mail deve conter entre 5 e 100 caracteres.");
        }
    }
}