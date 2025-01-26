using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;

namespace UseCases
{
    public class ConversaoUseCase(IConversaoGateway conversaoGateway, INotificador notificador) : BaseUseCase(notificador), IConversaoUseCase
    {
        public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(conversao);

            if (await conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken))
            {
                return true;
            }

            Notificar("Ocorreu um erro ao efetuar o upload do vídeo.");
            return false;
        }

        public async Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken) =>
            await conversaoGateway.ObterConversoesPorUsuarioAsync(usuarioId, cancellationToken);
    }
}
