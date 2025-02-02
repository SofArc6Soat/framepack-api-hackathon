using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
using Gateways.Dtos.Result;
using UseCases;

namespace Controllers
{
    public class ConversaoController(IConversaoUseCase conversaoUseCase) : IConversaoController
    {
        public async Task<bool> EfetuarUploadAsync(UploadRequestDto uploadRequestDto, string usuarioId, CancellationToken cancellationToken)
        {
            var conversao = new Conversao(Guid.NewGuid(), usuarioId, DateTime.Now, Status.AguardandoConversao, uploadRequestDto.NomeArquivo, uploadRequestDto.ArquivoVideo);

            return await conversaoUseCase.EfetuarUploadAsync(conversao, cancellationToken);
        }

        public async Task<List<ObterCoversoesResult>?> ObterConversoesPorUsuarioAsync(string usuarioId, CancellationToken cancellationToken)
        {
            var conversoes = await conversaoUseCase.ObterConversoesPorUsuarioAsync(usuarioId, cancellationToken);

            if (conversoes is null || conversoes.Count == 0)
            {
                return null;
            }

            var lista = new List<ObterCoversoesResult>();

            foreach (var item in conversoes)
            {
                lista.Add(new ObterCoversoesResult
                {
                    Id = item.Id,
                    Data = item.Data,
                    Status = item.Status.ToString(),
                    NomeArquivo = item.NomeArquivo
                });
            }

            return lista;
        }

        public async Task<Arquivo?> EfetuarDownloadAsync(string usuarioId, Guid conversaoId, CancellationToken cancellationToken) =>
            await conversaoUseCase.EfetuarDownloadAsync(usuarioId, conversaoId, cancellationToken);
    }
}
