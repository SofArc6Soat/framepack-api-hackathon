using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Dtos.Response;

namespace UseCases
{
    public class UsuarioUseCase(ICognitoGateway cognitoGateway, INotificador notificador) : BaseUseCase(notificador), IUsuarioUseCase
    {
        public async Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (ExecutarValidacao(new ValidarUsuario(), usuario)
                   && await cognitoGateway.CriarUsuarioAsync(usuario, senha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro ao cadastrar o usuário com o e-mail: {usuario.Email}, este e-mail já está sendo utilizado.");
            return false;
        }

        public async Task<TokenUsuario?> IdentificarUsuarioAsync(string email, string senha, CancellationToken cancellationToken)
        {
            var token = await cognitoGateway.IdentifiqueSeAsync(email, senha, cancellationToken);

            if (token is null)
            {
                Notificar("Email ou senha inválidos.");
                return token;
            }

            return token;
        }

        public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(emailVerificacao);

            if (ExecutarValidacao(new ValidarEmailVerificacao(), emailVerificacao)
                && await cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar a verificação do e-mail: {emailVerificacao.Email}");
            return false;
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recuperacaoSenha);

            if (ExecutarValidacao(new ValidarSolicitacaoRecuperacaoSenha(), recuperacaoSenha)
                && await cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro solicitar a recuperacao de senha do e-mail: {recuperacaoSenha.Email}");
            return false;
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(resetSenha);

            if (ExecutarValidacao(new ValidarResetSenha(), resetSenha)
                && await cognitoGateway.EfetuarResetSenhaAsync(resetSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar o reset de senha do e-mail: {resetSenha.Email}");
            return false;
        }
    }
}
