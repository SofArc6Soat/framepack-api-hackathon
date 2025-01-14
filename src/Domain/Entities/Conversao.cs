using Core.Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Entities
{
    public class Conversao : Entity, IAggregateRoot
    {
        public Guid UsuarioId { get; private set; }
        public DateTime Data { get; private set; }
        public Status Status { get; private set; }
        public string NomeArquivo { get; private set; }

        public string? UrlArquivoVideo { get; private set; }
        public string? UrlArquivoCompactado { get; private set; }

        public Conversao(Guid id, Guid usuarioId, DateTime data, Status status, string nomeArquivo, string urlArquivoVideo, string urlArquivoCompactado)
        {
            Id = id;
            UsuarioId = usuarioId;
            Data = data;
            Status = status;
            NomeArquivo = nomeArquivo;
            UrlArquivoVideo = urlArquivoVideo;
            UrlArquivoCompactado = urlArquivoCompactado;
        }
    }

    public class ValidarConversao : AbstractValidator<Conversao>
    {
        public ValidarConversao()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.UsuarioId)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.Data)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.NomeArquivo)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .Length(2, 50).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");
        }
    }
}