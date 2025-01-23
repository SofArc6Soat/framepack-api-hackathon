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

            return CustomResponsePost($"conversoes/{request.Id}", request, result);
        }
    }
}
