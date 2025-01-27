using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Cognito.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("usuarios")]
    public class UsuariosApiController(IUsuarioController usuarioController, INotificador notificador) : MainController(notificador)
    {
        [HttpPost]
        public async Task<IActionResult> CadastrarUsuarioAsync(UsuarioRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.CadastrarUsuarioAsync(request, cancellationToken);

            request.Senha = "*******";

            return CustomResponsePost($"usuarios/{request.Id}", request, result);
        }

        [HttpPost("identifique-se")]
        public async Task<IActionResult> IdentificarUsuario(IdentifiqueSeRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.IdentificarUsuarioAsync(request, cancellationToken);

            request.Senha = "*******";

            return result == null
                    ? CustomResponsePost($"usuarios/identifique-se", request, false)
                    : CustomResponsePost($"usuarios/identifique-se", result, true);
        }

        [HttpPost("email-verificacao:confirmar")]
        public async Task<IActionResult> ConfirmarEmailVerificaoAsync([FromBody] ConfirmarEmailVerificacaoDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.ConfirmarEmailVerificacaoAsync(request, cancellationToken);

            return CustomResponsePost($"usuarios/email-verificacao:confirmar", request, result);
        }

        [HttpPost("esquecia-senha:solicitar")]
        public async Task<IActionResult> SolicitarRecuperacaoSenhaAsync([FromBody] SolicitarRecuperacaoSenhaDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.SolicitarRecuperacaoSenhaAsync(request, cancellationToken);

            return CustomResponsePost($"usuarios/esquecia-senha:solicitar", request, result);
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

            return CustomResponsePost($"usuarios/esquecia-senha:resetar", request, result);
        }
    }
}
