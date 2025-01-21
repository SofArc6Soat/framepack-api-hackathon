using Amazon.DynamoDBv2.DataModel;
using Domain.Entities;
using Infra.Dto;

namespace Gateways
{
    public class ConversaoGateway(IDynamoDBContext repository) : IConversaoGateway
    {
        public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
        {
            // TODO: Efetuar upload S3

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

            // TODO: Gerar evento

            return true;
        }
    }
}
