using Domain.ValueObjects;

namespace Gateways.Dtos.Result
{
    public record ObterCoversoesResult
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public Status Status { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string UrlArquivoCompactado { get; set; } = string.Empty;
    }
}
