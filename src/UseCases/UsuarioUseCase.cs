using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito;
using Gateways.Cognito.Dtos.Response;

namespace UseCases;

public class UsuarioUseCase : BaseUseCase, IUsuarioUseCase
{
    private readonly ICognitoGateway _cognitoGateway;

    public UsuarioUseCase(ICognitoGateway cognitoGateway, INotificador notificador) : base(notificador)
    {
        _cognitoGateway = cognitoGateway;
    }

    public async Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        if (ExecutarValidacao(new ValidarUsuario(), usuario)
               && await _cognitoGateway.CriarUsuarioAsync(usuario, senha, cancellationToken))
        {
            return true;
        }

        Notificar($"Ocorreu um erro ao cadastrar o usuário com o e-mail: {usuario.Email}, este e-mail já está sendo utilizado.");
        return false;
    }

    public async Task<TokenUsuario?> IdentificarUsuarioAsync(string email, string senha, CancellationToken cancellationToken) =>
        await _cognitoGateway.IdentifiqueSeAsync(email, senha, cancellationToken);

    public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(emailVerificacao);

        if (ExecutarValidacao(new ValidarEmailVerificacao(), emailVerificacao)
            && await _cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken))
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
            && await _cognitoGateway.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken))
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
            && await _cognitoGateway.EfetuarResetSenhaAsync(resetSenha, cancellationToken))
        {
            return true;
        }

        Notificar($"Ocorreu um erro efetuar o reset de senha do e-mail: {resetSenha.Email}");
        return false;
    }
}
