using Domain.Entities;
using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IClienteController
    {
        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(ClienteAtualizarRequestDto clienteAtualizarRequestDto, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);
    }
}
