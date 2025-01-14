using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using UseCases;

namespace Controllers
{
    public class UsuarioController(IUsuarioUseCase usuarioUseCase) : IUsuarioController
    {
        public async Task<bool> CadastrarUsuarioAsync(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken)
        {
            var ususario = new Usuario(usuarioRequestDto.Id, usuarioRequestDto.Nome, usuarioRequestDto.Email);

            return await usuarioUseCase.CadastrarUsuarioAsync(ususario, usuarioRequestDto.Senha, cancellationToken);
        }

        public async Task<TokenUsuario?> IdentificarUsuarioAsync(IdentifiqueSeRequestDto identifiqueSeRequestDto, CancellationToken cancellationToken) =>
            await usuarioUseCase.IdentificarUsuarioAsync(identifiqueSeRequestDto.Email, identifiqueSeRequestDto.Senha, cancellationToken);

        public async Task<bool> ConfirmarEmailVerificacaoAsync(ConfirmarEmailVerificacaoDto confirmarEmailVerificacaoDto, CancellationToken cancellationToken)
        {
            var emailVerificacao = new EmailVerificacao(confirmarEmailVerificacaoDto.Email, confirmarEmailVerificacaoDto.CodigoVerificacao);

            return await usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken);
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenhaDto solicitarRecuperacaoSenha, CancellationToken cancellationToken)
        {
            var recuperacaoSenha = new RecuperacaoSenha(solicitarRecuperacaoSenha.Email);

            return await usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken);
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetarSenhaDto resetarSenhaDto, CancellationToken cancellationToken)
        {
            var resetarSenha = new ResetSenha(resetarSenhaDto.Email, resetarSenhaDto.CodigoVerificacao, resetarSenhaDto.NovaSenha);

            return await usuarioUseCase.EfetuarResetSenhaAsync(resetarSenha, cancellationToken);
        }
    }
}
