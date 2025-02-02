using Domain.Entities;

namespace Gateways
{
    public interface IConversaoGateway
    {
        Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
        Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(string usuarioId, CancellationToken cancellationToken);
        Task<Conversao?> ObterConversaoAsync(string usuarioId, Guid conversaoId, CancellationToken cancellationToken);
        Task<Arquivo?> EfetuarDownloadAsync(Conversao conversao, CancellationToken cancellationToken);
    }
}