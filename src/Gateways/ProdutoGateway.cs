//using Core.Infra.MessageBroker;
//using Domain.Entities;
//using Domain.ValueObjects;
//using Gateways.Dtos.Events;
//using Infra.Dto;
//using Infra.Repositories;

//namespace Gateways
//{
//    public class ProdutoGateway(IProdutoRepository produtoRepository, ISqsService<ProdutoCriadoEvent> sqsProdutoCriado, ISqsService<ProdutoAtualizadoEvent> sqsProdutoAtualizado, ISqsService<ProdutoExcluidoEvent> sqsProdutoExcluido) : IProdutoGateway
//    {
//        public async Task<bool> CadastrarProdutoAsync(Produto produto, CancellationToken cancellationToken)
//        {
//            var produtoDto = new ProdutoDb
//            {
//                Id = produto.Id,
//                Nome = produto.Nome,
//                Descricao = produto.Descricao,
//                Preco = produto.Preco,
//                Categoria = produto.Categoria.ToString(),
//                Ativo = produto.Ativo
//            };

//            await produtoRepository.InsertAsync(produtoDto, cancellationToken);

//            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken) && await sqsProdutoCriado.SendMessageAsync(GerarProdutoCriadoEvent(produtoDto));
//        }

//        public async Task<bool> AtualizarProdutoAsync(Produto produto, CancellationToken cancellationToken)
//        {
//            var produtoDto = new ProdutoDb
//            {
//                Id = produto.Id,
//                Nome = produto.Nome,
//                Descricao = produto.Descricao,
//                Preco = produto.Preco,
//                Categoria = produto.Categoria.ToString(),
//                Ativo = produto.Ativo
//            };

//            await produtoRepository.UpdateAsync(produtoDto, cancellationToken);

//            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken) && await sqsProdutoAtualizado.SendMessageAsync(GerarProdutoAtualizadoEvent(produtoDto));
//        }

//        public async Task<bool> DeletarProdutoAsync(Guid id, CancellationToken cancellationToken)
//        {
//            await produtoRepository.DeleteAsync(id, cancellationToken);

//            return await produtoRepository.UnitOfWork.CommitAsync(cancellationToken) && await sqsProdutoExcluido.SendMessageAsync(GerarProdutoExcluidoEvent(id));
//        }

//        public bool VerificarProdutoExistente(Guid id, string nome, string descricao, CancellationToken cancellationToken)
//        {
//            var produtoExistente = produtoRepository.Find(e => e.Id == id || e.Nome == nome || e.Descricao == descricao, cancellationToken).FirstOrDefault(g => g.Id == id);

//            return produtoExistente is not null;
//        }

//        public async Task<bool> VerificarProdutoExistenteAsync(Guid id, CancellationToken cancellationToken)
//        {
//            var produtoExistente = await produtoRepository.FindByIdAsync(id, cancellationToken);

//            return produtoExistente is not null;
//        }

//        public async Task<Produto?> ObterProdutoAsync(Guid id, CancellationToken cancellationToken)
//        {
//            var produtoDto = await produtoRepository.FindByIdAsync(id, cancellationToken);

//            if (produtoDto is null)
//            {
//                return null;
//            }

//            _ = Enum.TryParse(produtoDto.Categoria, out Categoria categoria);
//            return new Produto(produtoDto.Id, produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, categoria, produtoDto.Ativo);
//        }

//        public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync(CancellationToken cancellationToken)
//        {
//            var produtoDto = await produtoRepository.ObterTodosProdutosAsync(cancellationToken);

//            if (produtoDto.Any())
//            {
//                var produto = new List<Produto>();
//                foreach (var item in produtoDto)
//                {
//                    _ = Enum.TryParse(item.Categoria, out Categoria produtoCategoria);

//                    produto.Add(new Produto(item.Id, item.Nome, item.Descricao, item.Preco, produtoCategoria, item.Ativo));
//                }

//                return produto;
//            }

//            return [];
//        }

//        public async Task<IEnumerable<Produto>> ObterProdutosCategoriaAsync(Categoria categoria, CancellationToken cancellationToken)
//        {
//            var produtoDto = await produtoRepository.ObterProdutosCategoriaAsync(categoria.ToString(), cancellationToken);

//            if (produtoDto.Any())
//            {
//                var produto = new List<Produto>();
//                foreach (var item in produtoDto)
//                {
//                    _ = Enum.TryParse(item.Categoria, out Categoria produtoCategoria);

//                    produto.Add(new Produto(item.Id, item.Nome, item.Descricao, item.Preco, produtoCategoria, item.Ativo));
//                }

//                return produto;
//            }

//            return [];
//        }

//        private static ProdutoCriadoEvent GerarProdutoCriadoEvent(ProdutoDb produtoDb) => new()
//        {
//            Id = produtoDb.Id,
//            Nome = produtoDb.Nome,
//            Descricao = produtoDb.Descricao,
//            Preco = produtoDb.Preco,
//            Categoria = produtoDb.Categoria.ToString(),
//            Ativo = produtoDb.Ativo
//        };

//        private static ProdutoAtualizadoEvent GerarProdutoAtualizadoEvent(ProdutoDb produtoDb) => new()
//        {
//            Id = produtoDb.Id,
//            Nome = produtoDb.Nome,
//            Descricao = produtoDb.Descricao,
//            Preco = produtoDb.Preco,
//            Categoria = produtoDb.Categoria.ToString(),
//            Ativo = produtoDb.Ativo
//        };

//        private static ProdutoExcluidoEvent GerarProdutoExcluidoEvent(Guid id) => new()
//        {
//            Id = id
//        };
//    }
//}
