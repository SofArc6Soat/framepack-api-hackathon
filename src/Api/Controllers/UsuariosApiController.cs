using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Cognito.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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

        if (!result)
        {
            return BadRequest("Falha ao cadastrar usuario.");
        }

        return Created($"usuarios/{request.Id}", new { success = result });
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

        if (result == null)
        {
            return BadRequest("Falha ao identificar usuario.");
        }

        return Created($"usuarios/identifique-se", new { success = true });
    }

    [HttpPost("email-verificacao:confirmar")]
    public async Task<IActionResult> ConfirmarEmailVerificaoAsync([FromBody] ConfirmarEmailVerificacaoDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ErrorBadRequestModelState(ModelState);
        }

        var result = await usuarioController.ConfirmarEmailVerificacaoAsync(request, cancellationToken);

        if (!result)
        {
            return BadRequest("Falha ao confirmar verificacao de e-mail.");
        }

        return Created($"usuarios/email-verificacao:confirmar", new { success = result });
    }

    [HttpPost("esquecia-senha:solicitar")]
    public async Task<IActionResult> SolicitarRecuperacaoSenhaAsync([FromBody] SolicitarRecuperacaoSenhaDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ErrorBadRequestModelState(ModelState);
        }

        var result = await usuarioController.SolicitarRecuperacaoSenhaAsync(request, cancellationToken);

        if (!result)
        {
            return BadRequest("Falha ao solicitar recuperacao de senha.");
        }

        return Created($"usuarios/esquecia-senha:solicitar", new { success = result });
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

        if (!result)
        {
            return BadRequest("Falha ao resetar senha.");
        }

        return Created($"usuarios/esquecia-senha:resetar", new { success = result });
    }
}
