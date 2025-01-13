using Domain.Entities;

namespace Gateways
{
    public interface IClienteGateway
    {
        bool VerificarClienteExistente(Guid id, string? cpf, string? email, CancellationToken cancellationToken);
        Task<Cliente?> VerificarClienteExistenteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CadastrarClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken);
        Task<bool> AtualizarClienteAsync(Cliente cliente, CancellationToken cancellationToken);
        Task<bool> DeletarClienteAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<Cliente>> ObterTodosClientesAsync(CancellationToken cancellationToken);
    }
}
