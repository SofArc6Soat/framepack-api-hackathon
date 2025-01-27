using Domain.Entities;
using Gateways.Dtos.Request;
using Gateways.Dtos.Result;

namespace Controllers
{
    public interface IConversaoController
    {
        Task<bool> EfetuarUploadAsync(UploadRequestDto uploadRequestDto, CancellationToken cancellationToken);
        Task<List<ObterCoversoesResult>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
        Task<Arquivo?> EfetuarDownloadAsync(Guid usuarioId, Guid conversaoId, CancellationToken cancellationToken);

    }
}
