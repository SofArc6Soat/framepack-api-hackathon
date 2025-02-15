﻿using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Gateways.Cognito;

namespace UseCases
{
    public class ConversaoUseCase(IConversaoGateway conversaoGateway, ICognitoGateway cognitoGateway, INotificador notificador) : BaseUseCase(notificador), IConversaoUseCase
    {
        public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(conversao);

            var usuarioCognito = await cognitoGateway.ObertUsuarioCognitoPorIdAsync(conversao.UsuarioId, cancellationToken);
            var emailUsuario = usuarioCognito.UserAttributes.FirstOrDefault(attr => attr.Name == "email");

            if (emailUsuario is null)
            {
                Notificar("Não foi possível identificar o email do usuário.");
                return false;
            }

            conversao.SetEmailUsuario(emailUsuario.Value);

            if (await conversaoGateway.EfetuarUploadAsync(conversao, cancellationToken))
            {
                return true;
            }

            Notificar("Ocorreu um erro ao efetuar o upload do vídeo.");
            return false;
        }

        public async Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(string usuarioId, CancellationToken cancellationToken) =>
            await conversaoGateway.ObterConversoesPorUsuarioAsync(usuarioId, cancellationToken);

        public async Task<Arquivo?> EfetuarDownloadAsync(string usuarioId, Guid conversaoId, CancellationToken cancellationToken)
        {
            var conversao = await conversaoGateway.ObterConversaoAsync(usuarioId, conversaoId, cancellationToken);

            if (conversao is null)
            {
                Notificar("Conversao Inexistente");
                return null;
            }

            if (string.IsNullOrEmpty(conversao.UrlArquivoCompactado))
            {
                Notificar("O arquivo compactado ainda não está disponível para download");
                return null;
            }

            var arquivoDownload = await conversaoGateway.EfetuarDownloadAsync(conversao, cancellationToken);

            if (arquivoDownload is null)
            {
                Notificar("Falha ao efetuar o download");
                return null;
            }

            return arquivoDownload;
        }
    }
}
