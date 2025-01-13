using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;

namespace UseCases
{
    public class ClienteUseCase(IClienteGateway clientesGateway, INotificador notificador) : BaseUseCase(notificador), IClienteUseCase
    {
        public async Task<bool> CadastrarClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            if (clientesGateway.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, cancellationToken))
            {
                Notificar("Cliente já existente");
                return false;
            }

            if (ExecutarValidacao(new ValidarCliente(), cliente)
                   && await clientesGateway.CadastrarClienteAsync(cliente, senha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro ao cadastrar o cliente com o e-mail: {cliente.Email}, este e-mail já está sendo utilizado.");
            return false;
        }

        public async Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            var clienteExistente = await clientesGateway.VerificarClienteExistenteAsync(cliente.Id, cancellationToken);

            if (clienteExistente is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            var clienteAualizar = new Cliente(clienteExistente.Id, cliente.Nome, clienteExistente.Email, clienteExistente.Cpf, cliente.Ativo);

            return ExecutarValidacao(new ValidarCliente(), clienteAualizar)
                   && await clientesGateway.AtualizarClienteAsync(clienteAualizar, cancellationToken);
        }

        public async Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken)
        {
            if (await clientesGateway.VerificarClienteExistenteAsync(id, cancellationToken) is null)
            {
                Notificar("Cliente inexistente");
                return false;
            }

            return await clientesGateway.DeletarClienteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken) =>
            await clientesGateway.ObterTodosClientesAsync(cancellationToken);
    }
}
