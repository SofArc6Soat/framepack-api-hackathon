using Domain.Entities;
using Gateways.Dtos.Result;

namespace Gateways;

public interface IConversaoGateway
{
    Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
    Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
}