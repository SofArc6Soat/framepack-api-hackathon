using Domain.Entities;

namespace UseCases
{
    public interface IConversaoUseCase
    {
        Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken);
    }
}
