using Amazon.DynamoDBv2.DataModel;
using Core.Infra.MessageBroker;
using Core.Infra.S3;
using Domain.Entities;
using Gateways.Dtos.Events;
using Infra.Dto;

namespace Gateways;

public class ConversaoGateway : IConversaoGateway
{
    private readonly IDynamoDBContext _repository;
    private readonly ISqsService<ConversaoSolicitadaEvent> _sqsService;
    private readonly IS3Service _s3Service;

    public ConversaoGateway(IDynamoDBContext repository, ISqsService<ConversaoSolicitadaEvent> sqsService, IS3Service s3Service)
    {
        _repository = repository;
        _sqsService = sqsService;
        _s3Service = s3Service;
    }

    public async Task<bool> EfetuarUploadAsync(Conversao conversao, CancellationToken cancellationToken)
    {
        var urlArquivoVideo = await _s3Service.UploadArquivoAsync(conversao.Id, conversao.ArquivoVideo);

        var preSignedUrl = _s3Service.GerarPreSignedUrl(urlArquivoVideo);

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

        await _repository.SaveAsync(conversaoDto, cancellationToken);

        return await _sqsService.SendMessageAsync(GerarConversaoSolicitadaEvent(conversaoDto));
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
}
