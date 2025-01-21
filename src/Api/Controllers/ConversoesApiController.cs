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

            // Upload para o S3
            // var s3Result = await _s3Service.UploadFileAsync(file);

            // Salvar metadados no DynamoDB
            //var metadata = new VideoMetadata
            //{
            //    FileName = file.FileName,
            //    ContentType = file.ContentType,
            //    S3Url = s3Result,
            //    UploadedAt = DateTime.UtcNow
            //};
            //await _dynamoDbService.SaveMetadataAsync(metadata);

            return CustomResponsePost($"conversoes/{request.Id}", request, result);
        }
    }
}
