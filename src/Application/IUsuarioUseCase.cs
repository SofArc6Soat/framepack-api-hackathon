using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Response;

namespace UseCases
{
    public interface IUsuarioUseCase
    {
        Task<TokenUsuario?> IdentificarFuncionarioAsync(string email, string senha, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentificarClienteCpfAsync(string cpf, string senha, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken);
    }
}