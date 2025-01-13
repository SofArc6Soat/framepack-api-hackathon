using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Dtos.Response;

namespace UseCases
{
    public class UsuarioUseCase(ICognitoGateway cognitoGateway, INotificador notificador) : BaseUseCase(notificador), IUsuarioUseCase
    {
        public async Task<TokenUsuario?> IdentificarFuncionarioAsync(string email, string senha, CancellationToken cancellationToken) =>
            await cognitoGateway.IdentifiqueSeAsync(email, null, senha, cancellationToken);

        public async Task<TokenUsuario?> IdentificarClienteCpfAsync(string cpf, string senha, CancellationToken cancellationToken) =>
            await cognitoGateway.IdentifiqueSeAsync(null, cpf, senha, cancellationToken);

        public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(emailVerificacao);

            if (await cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar a verificação do e-mail: {emailVerificacao.Email}");
            return false;
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recuperacaoSenha);

            if (await cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro solicitar a recuperacao de senha do e-mail: {recuperacaoSenha.Email}");
            return false;
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(resetSenha);

            if (await cognitoGateway.EfetuarResetSenhaAsync(resetSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar o reset de senha do e-mail: {resetSenha.Email}");
            return false;
        }
    }
}
