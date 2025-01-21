using Domain.Entities;
using Gateways;

namespace UseCases
{
    public class ConversaoUseCase(IConversaoGateway conversaoGateway) : IConversaoUseCase
    {
        public Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(conversao);

            // Validacoes adicionais

            return conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken);
        }
    }
}
