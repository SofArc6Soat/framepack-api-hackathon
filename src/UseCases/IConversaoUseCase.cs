using Domain.Entities;

namespace UseCases
{
    public interface IConversaoUseCase
    {
        Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
        Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(string usuarioId, CancellationToken cancellationToken);
        Task<Arquivo?> EfetuarDownloadAsync(string usuarioId, Guid conversaoId, CancellationToken cancellationToken);
    }
}
