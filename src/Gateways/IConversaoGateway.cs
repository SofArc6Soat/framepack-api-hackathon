using Domain.Entities;

namespace Gateways
{
    public interface IConversaoGateway
    {
        Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
    }
}
