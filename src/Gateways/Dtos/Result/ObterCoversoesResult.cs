namespace Gateways.Dtos.Result
{
    public record ObterCoversoesResult
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; } = string.Empty;
        public string NomeArquivo { get; set; } = string.Empty;
        public string UrlArquivoCompactado { get; set; } = string.Empty;
    }
}
