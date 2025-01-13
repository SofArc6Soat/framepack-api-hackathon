using Domain.Entities;
using Domain.ValueObjects;
namespace UseCases
{
    public interface IProdutoUseCase
    {
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria, CancellationToken cancellationToken);
        Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken);
        Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken);
        Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken);
    }
}
