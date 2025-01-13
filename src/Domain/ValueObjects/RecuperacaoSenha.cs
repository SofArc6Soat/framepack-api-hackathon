using Core.Domain.Entities;
using FluentValidation;

namespace Domain.ValueObjects
{
    public class RecuperacaoSenha : Entity, IAggregateRoot
    {
        public string Email { get; private set; }

        public RecuperacaoSenha(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
        }
    }

    public class ValidarSolicitacaoRecuperacaoSenha : AbstractValidator<RecuperacaoSenha>
    {
        public ValidarSolicitacaoRecuperacaoSenha() =>
            RuleFor(c => c.Email)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .EmailAddress().WithMessage("O {PropertyName} está em um formato inválido.")
                .Length(5, 100).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");
    }
}
