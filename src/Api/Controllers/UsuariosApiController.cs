using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Cognito.Dtos.Request;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("usuarios")]
    public class UsuariosApiController(IUsuarioController usuarioController, INotificador notificador) : MainController(notificador)
    {
        [HttpPost("cliente/identifique-se")]
        public async Task<IActionResult> IdentificarClienteCpf(ClienteIdentifiqueSeRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.IdentificarClienteCpfAsync(request, cancellationToken);

            request.Senha = "*******";

            return result == null
                ? CustomResponsePost($"usuarios/cliente/identifique-se", request, false)
                : CustomResponsePost($"usuarios/cliente/identifique-se", result, true);
        }

        [HttpPost("funcionario/identifique-se")]
        public async Task<IActionResult> IdentificarFuncionario(FuncinarioIdentifiqueSeRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.IdentificarFuncionarioAsync(request, cancellationToken);

            request.Senha = "*******";

            return result == null
                ? CustomResponsePost($"usuarios/funcionario/identifique-se", request, false)
                : CustomResponsePost($"usuarios/funcionario/identifique-se", result, true);
        }

        [HttpPost("email-verificacao:confirmar")]
        public async Task<IActionResult> ConfirmarEmailVerificaoAsync([FromBody] ConfirmarEmailVerificacaoDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.ConfirmarEmailVerificacaoAsync(request, cancellationToken);

            return CustomResponsePost($"usuarios/email-verificacao:confirmar", result, result);
        }

        [HttpPost("esquecia-senha:solicitar")]
        public async Task<IActionResult> SolicitarRecuperacaoSenhaAsync([FromBody] SolicitarRecuperacaoSenhaDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.SolicitarRecuperacaoSenhaAsync(request, cancellationToken);

            return CustomResponsePost($"usuarios/esquecia-senha:solicitar", result, result);
        }

        [HttpPost("esquecia-senha:resetar")]
        public async Task<IActionResult> EfetuarResetSenhaAsync([FromBody] ResetarSenhaDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.EfetuarResetSenhaAsync(request, cancellationToken);

            request.NovaSenha = "*******";

            return CustomResponsePost($"usuarios/esquecia-senha:resetar", result, result);
        }
    }
}
