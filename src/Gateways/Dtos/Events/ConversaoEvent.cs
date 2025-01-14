using Core.Domain.Entities;

namespace Gateways.Dtos.Events
{
    public record ConversaoCriada : Event
    {
        public Guid UsuarioId { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string NomeArquivo { get; set; } = string.Empty;
        public string UrlArquivoVideo { get; set; } = string.Empty;
    }
}
