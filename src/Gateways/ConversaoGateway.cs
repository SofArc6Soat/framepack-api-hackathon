using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Core.Infra.MessageBroker;
using Core.Infra.S3;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Events;
using Infra.Dto;

namespace Gateways
{
    public class ConversaoGateway(IDynamoDBContext repository, ISqsService<ConversaoSolicitadaEvent> sqsService, IS3Service s3Service) : IConversaoGateway
    {
        public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            var urlArquivoVideo = await s3Service.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo);

            var preSignedUrl = s3Service.GerarPreSignedUrl(urlArquivoVideo);

            if (string.IsNullOrEmpty(preSignedUrl))
            {
                return false;
            }

            conversao.SetUrlArquivoVideo(preSignedUrl);

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

        public async Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)
        {
            var conditions = new List<ScanCondition>
                {
                    new("UsuarioId", ScanOperator.Equal, usuarioId)
                };

            var conversaoDb = await repository.ScanAsync<ConversaoDb>(conditions).GetRemainingAsync(cancellationToken);

            return conversaoDb.Select(item => ToConversao(item)).ToList();
        }

        private static ConversaoSolicitadaEvent GerarConversaoSolicitadaEvent(ConversaoDb conversaoDto) => new()
        {
            Id = conversaoDto.Id,
            UsuarioId = conversaoDto.UsuarioId,
            Data = conversaoDto.Data,
            Status = conversaoDto.Status,
            NomeArquivo = conversaoDto.NomeArquivo,
            UrlArquivoVideo = conversaoDto.UrlArquivoVideo
        };

        private static Conversao ToConversao(ConversaoDb conversaoDb)
        {
            var status = (Status)Enum.Parse(typeof(Status), conversaoDb.Status, ignoreCase: true);

            return new Conversao(conversaoDb.Id, conversaoDb.UsuarioId, conversaoDb.Data, status, conversaoDb.NomeArquivo, conversaoDb.UrlArquivoVideo, conversaoDb.UrlArquivoCompactado);
        }
    }
}
