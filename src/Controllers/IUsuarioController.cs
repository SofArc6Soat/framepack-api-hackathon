using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IUsuarioController
    {
        Task<TokenUsuario?> IdentificarClienteCpfAsync(ClienteIdentifiqueSeRequestDto clienteIdentifiqueSeRequestDto, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentificarFuncionarioAsync(FuncinarioIdentifiqueSeRequestDto funcinarioIdentifiqueSeRequestDto, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(ConfirmarEmailVerificacaoDto confirmarEmailVerificacaoDto, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenhaDto solicitarRecuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetarSenhaDto resetarSenhaDto, CancellationToken cancellationToken);
    }
}
