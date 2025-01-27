using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
using Gateways.Dtos.Result;
using UseCases;

namespace Controllers
{
    public class ConversaoController(IConversaoUseCase conversaoUseCase) : IConversaoController
    {
        public async Task<bool> EfetuarUploadAsync(UploadRequestDto uploadRequestDto, CancellationToken cancellationToken)
        {
            var conversao = new Conversao(Guid.NewGuid(), uploadRequestDto.UsuarioId, DateTime.Now, Status.AguardandoConversao, uploadRequestDto.NomeArquivo, uploadRequestDto.ArquivoVideo);

            return await conversaoUseCase.EfetuarUploadAsync(conversao, cancellationToken);
        }

        public async Task<List<ObterCoversoesResult>?> ObterConversoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)
        {
            var conversoes = await conversaoUseCase.ObterConversoesPorUsuarioAsync(usuarioId, cancellationToken);

            var lista = new List<ObterCoversoesResult>([]);

            if (conversoes is not null)
            {
                foreach (var item in conversoes)
                {
                    lista.Add(new ObterCoversoesResult
                    {
                        Id = item.Id,
                        Data = item.Data,
                        Status = item.Status.ToString(),
                        NomeArquivo = item.NomeArquivo,
                        UrlArquivoCompactado = item.UrlArquivoCompactado
                    });
                }
            }

            return lista;
        }

        public async Task<Arquivo?> EfetuarDownloadAsync(Guid usuarioId, Guid conversaoId, CancellationToken cancellationToken) =>
            await conversaoUseCase.EfetuarDownloadAsync(usuarioId, conversaoId, cancellationToken);
    }
}
