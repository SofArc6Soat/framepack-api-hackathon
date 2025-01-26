using Domain.Entities;
using Gateways.Dtos.Result;

namespace UseCases
{
    public interface IConversaoUseCase
    {
        Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
        Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    }
}
