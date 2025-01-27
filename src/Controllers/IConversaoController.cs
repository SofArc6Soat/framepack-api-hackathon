using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IConversaoController
    {
        Task<bool> EfetuarUploadAsync(UploadRequestDto uploadRequestDto, CancellationToken cancellationToken);
    }
}
