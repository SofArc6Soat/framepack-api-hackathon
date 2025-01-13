using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IFuncionarioController
    {
        Task<bool> CadastrarFuncionarioAsync(FuncionarioRequestDto funcionarioRequestDto, CancellationToken cancellationToken);
    }
}
