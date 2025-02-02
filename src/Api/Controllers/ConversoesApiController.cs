using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "UsuarioRole")]
    [Route("conversoes")]
    public class ConversoesApiController(IConversaoController conversaoController, INotificador notificador, IUserContextService userContext) : MainController(notificador)
    {
        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> EfetuarUploadAsync(UploadRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await conversaoController.EfetuarUploadAsync(request, userContext.UserId, cancellationToken);

            return CustomResponsePost($"conversoes", request, result);
        }

        [HttpGet]
        public async Task<IActionResult> ObterConversoesPorUsuarioAsync(CancellationToken cancellationToken)
        {
            var result = await conversaoController.ObterConversoesPorUsuarioAsync(userContext.UserId, cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("download/{conversaoId:guid}")]
        public async Task<IActionResult> EfetuarDownloadAsync([FromRoute] Guid conversaoId, CancellationToken cancellationToken)
        {
            var result = await conversaoController.EfetuarDownloadAsync(userContext.UserId, conversaoId, cancellationToken);

            return result is null ? NotFound() : File(result.BytesArquivo, "application/zip", result.NomeArquivo);
        }
    }
}
