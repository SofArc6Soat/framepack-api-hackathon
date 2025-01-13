using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Gateways.Dtos.Request;
using UseCases;

namespace Controllers
{
    public class UsuarioController(IUsuarioUseCase usuarioUseCase) : IUsuarioController
    {
        public async Task<TokenUsuario?> IdentificarClienteCpfAsync(ClienteIdentifiqueSeRequestDto clienteIdentifiqueSeRequestDto, CancellationToken cancellationToken) =>
            await usuarioUseCase.IdentificarClienteCpfAsync(clienteIdentifiqueSeRequestDto.Cpf, clienteIdentifiqueSeRequestDto.Senha, cancellationToken);

        public async Task<TokenUsuario?> IdentificarFuncionarioAsync(FuncinarioIdentifiqueSeRequestDto funcinarioIdentifiqueSeRequestDto, CancellationToken cancellationToken) =>
            await usuarioUseCase.IdentificarFuncionarioAsync(funcinarioIdentifiqueSeRequestDto.Email, funcinarioIdentifiqueSeRequestDto.Senha, cancellationToken);

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
