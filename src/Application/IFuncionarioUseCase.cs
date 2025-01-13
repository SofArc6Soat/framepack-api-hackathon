using Domain.Entities;

namespace UseCases
{
    public interface IFuncionarioUseCase
    {
        Task<bool> CadastrarFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken);
    }
}