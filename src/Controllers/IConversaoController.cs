using Gateways.Dtos.Request;
using Gateways.Dtos.Result;

namespace Controllers
{
    public interface IConversaoController
    {
        Task<bool> EfetuarUploadAsync(UploadRequestDto uploadRequestDto, CancellationToken cancellationToken);
        Task<List<ObterCoversoesResult>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    }
}
