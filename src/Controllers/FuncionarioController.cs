using Domain.Entities;
using Gateways.Dtos.Request;
using UseCases;

namespace Controllers
{
    public class FuncionarioController(IFuncionarioUseCase funcionarioUseCase) : IFuncionarioController
    {
        public async Task<bool> CadastrarFuncionarioAsync(FuncionarioRequestDto funcionarioRequestDto, CancellationToken cancellationToken)
        {
            var funcionario = new Funcionario(funcionarioRequestDto.Id, funcionarioRequestDto.Nome, funcionarioRequestDto.Email, funcionarioRequestDto.Ativo);

            return await funcionarioUseCase.CadastrarFuncionarioAsync(funcionario, funcionarioRequestDto.Senha, cancellationToken);
        }
    }
}
