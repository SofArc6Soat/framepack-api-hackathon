using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Dtos.Request;
using UseCases;

namespace Controllers
{
    public class ProdutoController(IProdutoUseCase produtoUseCase) : IProdutoController
    {
        public async Task<bool> CadastrarProdutoAsync(ProdutoRequestDto produtoDto, CancellationToken cancellationToken)
        {
            var categoriaValida = Enum.TryParse<Categoria>(produtoDto.Categoria, out var categoria);

            if (categoriaValida)
            {
                var produto = new Produto(produtoDto.Id, produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, categoria, produtoDto.Ativo);

                return await produtoUseCase.CadastrarProdutoAsync(produto, cancellationToken);
            }

            return false;
        }

        public async Task<bool> AtualizarProdutoAsync(ProdutoRequestDto produtoDto, CancellationToken cancellationToken)
        {
            var categoriaValida = Enum.TryParse<Categoria>(produtoDto.Categoria, out var categoria);

            if (categoriaValida)
            {
                var produto = new Produto(produtoDto.Id, produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, categoria, produtoDto.Ativo);

                return await produtoUseCase.AtualizarProdutoAsync(produto, cancellationToken);
            }

            return false;
        }

        public async Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken) =>
            await produtoUseCase.DeletarProdutoAsync(id, cancellationToken);

        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken) =>
            await produtoUseCase.ObterTodosProdutosAsync(cancellationToken);

        public async Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(string categoriaDto, CancellationToken cancellationToken)
        {
            var categoriaValida = Enum.TryParse<Categoria>(categoriaDto, out var categoria);

            return categoriaValida ? await produtoUseCase.ObterProdutosCategoriaAsync(categoria, cancellationToken) : ([]);
        }
    }
}
