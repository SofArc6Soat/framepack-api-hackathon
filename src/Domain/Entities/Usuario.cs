using Core.Domain.Entities;
using FluentValidation;

namespace Domain.Entities
{
    public class Usuario : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public Usuario(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }
    }

    public class ValidarUsuario : AbstractValidator<Usuario>
    {
        public ValidarUsuario()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("O {PropertyName} n�o pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser v�lido.");

            RuleFor(c => c.Nome)
                .NotNull().WithMessage("O {PropertyName} n�o pode ser nulo.")
                .Length(2, 50).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");

            RuleFor(c => c.Email)
                .NotNull().WithMessage("O {PropertyName} n�o pode ser nulo.")
                .EmailAddress().WithMessage("O {PropertyName} est� em um formato inv�lido.")
                .Length(2, 100).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");
        }
    }
}
