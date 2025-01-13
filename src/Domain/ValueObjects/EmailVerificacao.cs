using Core.Domain.Entities;
using FluentValidation;

namespace Domain.ValueObjects
{
    public class EmailVerificacao : Entity, IAggregateRoot
    {
        public string Email { get; private set; }
        public string CodigoVerificacao { get; private set; }

        public EmailVerificacao(string email, string codigoVerificacao)
        {
            Id = Guid.NewGuid();
            Email = email;
            CodigoVerificacao = codigoVerificacao;
        }
    }

    public class ValidarEmailVerificacao : AbstractValidator<EmailVerificacao>
    {
        public ValidarEmailVerificacao()
        {
            RuleFor(c => c.Email)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .EmailAddress().WithMessage("O {PropertyName} está em um formato inválido.")
                .Length(5, 100).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");

            RuleFor(c => c.CodigoVerificacao)
                .NotNull().WithMessage("O status não pode ser nulo.")
                .Length(6, 6).WithMessage("O {PropertyName} precisa ter {MaxLength} caracteres e foi informado {PropertyValue}.");
        }
    }
}
