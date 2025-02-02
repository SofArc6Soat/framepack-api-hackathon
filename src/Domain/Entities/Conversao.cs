using Core.Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class Conversao : Entity, IAggregateRoot
    {
        public string UsuarioId { get; private set; }
        public string EmailUsuario { get; private set; } = string.Empty;
        public DateTime Data { get; private set; }
        public Status Status { get; private set; }
        public string NomeArquivo { get; private set; }
        public IFormFile ArquivoVideo { get; private set; }

        public string UrlArquivoVideo { get; private set; } = string.Empty;
        public string UrlArquivoCompactado { get; private set; } = string.Empty;

        public Conversao(Guid id, string usuarioId, DateTime data, Status status, string nomeArquivo, IFormFile arquivoVideo)
        {
            Id = id;
            UsuarioId = usuarioId;
            Data = data;
            Status = status;
            NomeArquivo = NormalizarNomeArquivo(nomeArquivo);
            ArquivoVideo = arquivoVideo;
        }

        public Conversao(Guid id, string usuarioId, DateTime data, Status status, string nomeArquivo, string urlArquivoVideo, string urlArquivoCompactado)
        {
            Id = id;
            UsuarioId = usuarioId;
            Data = data;
            Status = status;
            NomeArquivo = NormalizarNomeArquivo(nomeArquivo);
            UrlArquivoVideo = urlArquivoVideo;
            UrlArquivoCompactado = urlArquivoCompactado;
        }

        public void SetUrlArquivoVideo(string urlArquivoVideo) =>
            UrlArquivoVideo = urlArquivoVideo;

        public void SetUrlArquivoCompactado(string urlArquivoCompactado) =>
            UrlArquivoCompactado = urlArquivoCompactado;

        private static string NormalizarNomeArquivo(string nomeArquivo)
        {
            var normalizado = nomeArquivo.Replace(" ", "_");
            normalizado = Regex.Replace(normalizado, @"[^a-zA-Z0-9_.]", "");
            normalizado = Regex.Replace(normalizado, @"_+", "_");
            return normalizado.ToLowerInvariant();
        }

        public void SetEmailUsuario(string emailUsuario) =>
            EmailUsuario = emailUsuario;
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