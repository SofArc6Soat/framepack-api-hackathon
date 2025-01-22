using Amazon.DynamoDBv2.DataModel;
using Core.Infra.MessageBroker;
using Core.Infra.S3;
using Domain.Entities;
using Gateways.Dtos.Events;
using Infra.Dto;

namespace Gateways
{
    public class ConversaoGateway(IDynamoDBContext repository, ISqsService<ConversaoSolicitada> sqsService, S3Service s3Service) : IConversaoGateway
    {
        public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            var urlArquivoVideo = await s3Service.UploadFileAsync(conversao.ArquivoVideo, conversao.UsuarioId);

            if (string.IsNullOrEmpty(urlArquivoVideo))
            {
                return false;
            }

            conversao.SetUrlArquivoVideo(urlArquivoVideo);

            var conversaoDto = new ConversaoDb
            {
                Id = conversao.Id,
                UsuarioId = conversao.UsuarioId,
                Status = conversao.Status.ToString(),
                Data = conversao.Data,
                NomeArquivo = conversao.NomeArquivo,
                UrlArquivoVideo = conversao.UrlArquivoVideo,
                UrlArquivoCompactado = conversao.UrlArquivoCompactado
            };

            await repository.SaveAsync(conversaoDto, cancellationToken);

            return await sqsService.SendMessageAsync(GerarConversaoSolicitadaEvent(conversaoDto));
        }

        private static ConversaoSolicitada GerarConversaoSolicitadaEvent(ConversaoDb conversaoDto) => new()
        {
            Id = conversaoDto.Id,
            UsuarioId = conversaoDto.UsuarioId,
            Data = conversaoDto.Data,
            Status = conversaoDto.Status,
            NomeArquivo = conversaoDto.NomeArquivo,
            UrlArquivoVideo = conversaoDto.UrlArquivoVideo
        };
    }
}
