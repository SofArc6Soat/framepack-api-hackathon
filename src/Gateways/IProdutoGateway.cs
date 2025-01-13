using Domain.Entities;
using Domain.ValueObjects;

namespace Gateways
{
    public interface IProdutoGateway
    {
        bool VerificarProdutoExistente(Guid id, string nome, string descricao, CancellationToken cancellationToken);
        Task<bool> VerificarProdutoExistenteAsync(Guid id, CancellationToken cancellationToken);
        Task<Produto?> ObterProdutoAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken);
        Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken);
        Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria, CancellationToken cancellationToken);
    }
}
