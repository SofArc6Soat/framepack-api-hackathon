using Domain.Entities;
using Gateways.Dtos.Request;
using Gateways.Dtos.Result;

namespace Controllers
{
    public interface IConversaoController
    {
        Task<bool> EfetuarUploadAsync(UploadRequestDto uploadRequestDto, string usuarioId, CancellationToken cancellationToken);
        Task<List<ObterCoversoesResult>?> ObterConversoesPorUsuarioAsync(string usuarioId, CancellationToken cancellationToken);
        Task<Arquivo?> EfetuarDownloadAsync(string usuarioId, Guid conversaoId, CancellationToken cancellationToken);

    }
}
