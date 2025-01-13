using Core.Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Entities
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public Categoria Categoria { get; private set; }
        public bool Ativo { get; private set; }

        public Produto(Guid id, string nome, string descricao, decimal preco, Categoria categoria, bool ativo)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Categoria = categoria;
            Ativo = ativo;
        }
    }

    public class ValidarProduto : AbstractValidator<Produto>
    {
        public ValidarProduto()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.Nome)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .Length(2, 40).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");

            RuleFor(c => c.Descricao)
                .NotNull().WithMessage("A {PropertyName} não pode ser nulo.")
                .Length(5, 200).WithMessage("A {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");

            RuleFor(c => c.Preco)
                .GreaterThanOrEqualTo(1).WithMessage("O {PropertyName} precisa ser maior ou igual a {ComparisonValue} e foi informado {PropertyValue}.")
                .LessThanOrEqualTo(9999).WithMessage("O {PropertyName} precisa ser menor ou igual a {ComparisonValue} e foi informado {PropertyValue}.");

            RuleFor(c => c.Categoria)
                .IsInEnum().WithMessage("A {PropertyName} é inválida.");

            RuleFor(c => c.Ativo)
                .NotNull().WithMessage("O status não pode ser nulo.");
        }
    }
}
