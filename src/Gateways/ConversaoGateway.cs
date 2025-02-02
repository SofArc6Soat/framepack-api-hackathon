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
    public class ConversaoGateway(IDynamoDBContext repository, ISqsService<ConversaoSolicitadaEvent> sqsService, ISqsService<DownloadEfetuadoEvent> sqsServiceDownload, IS3Service s3Service) : IConversaoGateway
    {
        public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            var urlArquivoVideo = await s3Service.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo);

            if (string.IsNullOrEmpty(urlArquivoVideo))
            {
                return false;
            }

            conversao.SetUrlArquivoVideo(urlArquivoVideo);

            var conversaoDto = new ConversaoDb
            {
                Id = conversao.Id,
                UsuarioId = conversao.UsuarioId,
                EmailUsuario = conversao.EmailUsuario,
                Status = conversao.Status.ToString(),
                Data = conversao.Data,
                NomeArquivo = conversao.NomeArquivo,
                UrlArquivoVideo = conversao.UrlArquivoVideo,
                UrlArquivoCompactado = conversao.UrlArquivoCompactado
            };

            await repository.SaveAsync(conversaoDto, cancellationToken);

            return await sqsService.SendMessageAsync(GerarConversaoSolicitadaEvent(conversaoDto));
        }

        public async Task<List<Conversao>?> ObterConversoesPorUsuarioAsync(string usuarioId, CancellationToken cancellationToken)
        {
            var conditions = new List<ScanCondition>
                {
                    new("UsuarioId", ScanOperator.Equal, usuarioId)
                };

            var conversaoDb = await repository.ScanAsync<ConversaoDb>(conditions).GetRemainingAsync(cancellationToken);

            return conversaoDb.Select(item => ToConversao(item)).ToList();
        }

        public async Task<Arquivo?> EfetuarDownloadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();

            var url = s3Service.GerarPreSignedUrl(conversao.UrlArquivoCompactado);

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Erro ao acessar o arquivo para download.");
            }

            var arquivoBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

            if (arquivoBytes is null)
            {
                return null;
            }

            await sqsServiceDownload.SendMessageAsync(GerarDownloadEfetuadoEvent(conversao));

            return new Arquivo(arquivoBytes, string.Concat(conversao.NomeArquivo, ".zip"));
        }

        public async Task<Conversao?> ObterConversaoAsync(string usuarioId, Guid conversaoId, CancellationToken cancellationToken)
        {
            var conditions = new List<ScanCondition>
                {
                    new("Id", ScanOperator.Equal, conversaoId),
                    new("UsuarioId", ScanOperator.Equal, usuarioId)
                };

            var conversaoDb = await repository.ScanAsync<ConversaoDb>(conditions).GetRemainingAsync(cancellationToken);

            var conversao = conversaoDb.FirstOrDefault();

            return conversao is not null ? ToConversao(conversao) : null;
        }

        private static ConversaoSolicitadaEvent GerarConversaoSolicitadaEvent(ConversaoDb conversaoDto) => new()
        {
            Id = conversaoDto.Id,
            UsuarioId = conversaoDto.UsuarioId,
            EmailUsuario = conversaoDto.EmailUsuario,
            Data = conversaoDto.Data,
            Status = conversaoDto.Status,
            NomeArquivo = conversaoDto.NomeArquivo,
            UrlArquivoVideo = conversaoDto.UrlArquivoVideo
        };

        private static DownloadEfetuadoEvent GerarDownloadEfetuadoEvent(Conversao conversaoDto) => new()
        {
            Id = conversaoDto.Id,
            UrlArquivoVideo = conversaoDto.UrlArquivoVideo
        };

        private static Conversao ToConversao(ConversaoDb conversaoDb)
        {
            var status = (Status)Enum.Parse(typeof(Status), conversaoDb.Status, ignoreCase: true);

            return new Conversao(conversaoDb.Id, conversaoDb.UsuarioId, conversaoDb.Data, status, conversaoDb.NomeArquivo, conversaoDb.UrlArquivoVideo, conversaoDb.UrlArquivoCompactado);
        }
    }
}
