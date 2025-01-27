using Domain.Entities;

namespace UseCases
{
    public interface IConversaoUseCase
    {
        Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
        Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
        Task<Arquivo?> EfetuarDownloadAsync(Guid usuarioId, Guid conversaoId, CancellationToken cancellationToken);
    }
}
