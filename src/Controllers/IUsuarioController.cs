using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;

namespace Controllers
{
    public interface IUsuarioController
    {
        Task<bool> CadastrarUsuarioAsync(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentificarUsuarioAsync(IdentifiqueSeRequestDto identifiqueSeRequestDto, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(ConfirmarEmailVerificacaoDto confirmarEmailVerificacaoDto, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenhaDto solicitarRecuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetarSenhaDto resetarSenhaDto, CancellationToken cancellationToken);
    }
}
