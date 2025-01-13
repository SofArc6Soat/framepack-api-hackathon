using Domain.Entities;
using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IProdutoController
    {
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(string categoriaDto, CancellationToken cancellationToken);
        Task<bool> CadastrarProdutoAsync(ProdutoRequestDto produtoDto, CancellationToken cancellationToken);
        Task<bool> AtualizarProdutoAsync(ProdutoRequestDto produtoDto, CancellationToken cancellationToken);
        Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken);
    }
}
