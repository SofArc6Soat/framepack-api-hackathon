using Amazon.CognitoIdentityProvider.Model;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Response;

namespace Gateways.Cognito
{
    public interface ICognitoGateway
    {
        Task<bool> CriarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentifiqueSeAsync(string? email, string senha, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken);
        Task<AdminGetUserResponse> ObertUsuarioCognitoPorIdAsync(string userId, CancellationToken cancellationToken);
    }
}
