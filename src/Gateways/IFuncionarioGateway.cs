using Domain.Entities;

namespace Gateways
{
    public interface IFuncionarioGateway
    {
        bool VerificarFuncionarioExistente(Guid id, string? email, CancellationToken cancellationToken);
        Task<bool> CadastrarFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken);
    }
}
