using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;

namespace UseCases
{
    public class FuncionarioUseCase(IFuncionarioGateway funcionarioGateway, INotificador notificador) : BaseUseCase(notificador), IFuncionarioUseCase
    {
        public async Task<bool> CadastrarFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(funcionario);

            if (funcionarioGateway.VerificarFuncionarioExistente(funcionario.Id, funcionario.Email, cancellationToken))
            {
                Notificar("Funcionario já existente");
                return false;
            }

            if (ExecutarValidacao(new ValidarFuncionario(), funcionario)
                   && await funcionarioGateway.CadastrarFuncionarioAsync(funcionario, senha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro ao cadastrar o funcionario com o e-mail: {funcionario.Email}, este e-mail já está sendo utilizado.");
            return false;
        }
    }
}
