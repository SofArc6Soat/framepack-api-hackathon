using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("conversoes")]
    public class ConversoesApiController(IConversaoController conversaoController, INotificador notificador) : MainController(notificador)
    {
        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> EfetuarUploadAsync(UploadRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await conversaoController.EfetuarUploadAsync(request, cancellationToken);

            return CustomResponsePost($"conversoes", request, result);
        }

        [HttpGet("{usuarioId:guid}")]
        public async Task<IActionResult> ObterConversoesPorUsuarioAsync([FromRoute] Guid usuarioId, CancellationToken cancellationToken)
        {
            var result = await conversaoController.ObterConversoesPorUsuarioAsync(usuarioId, cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("download/{usuarioId:guid}/{conversaoId:guid}")]
        public async Task<IActionResult> EfetuarDownloadAsync([FromRoute] Guid usuarioId, [FromRoute] Guid conversaoId, CancellationToken cancellationToken)
        {
            var result = await conversaoController.EfetuarDownloadAsync(usuarioId, conversaoId, cancellationToken);

            return result is null ? NotFound() : File(result.BytesArquivo, "application/zip", result.NomeArquivo);
        }
    }
}
