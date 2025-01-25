using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
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
    }
}
