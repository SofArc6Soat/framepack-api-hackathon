using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //[Authorize(Policy = "AdminRole")]
    [AllowAnonymous]
    [Route("produtos")]
    public class ProdutosApiController(IProdutoController produtosController, INotificador notificador) : MainController(notificador)
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObterTodosProdutos(CancellationToken cancellationToken)
        {
            var result = await produtosController.ObterTodosProdutosAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [AllowAnonymous]
        [HttpGet("categoria")]
        public async Task<IActionResult> ObterProdutosCategoria(string categoria, CancellationToken cancellationToken)
        {
            var result = await produtosController.ObterProdutosCategoriaAsync(categoria, cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProduto(ProdutoRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await produtosController.CadastrarProdutoAsync(request, cancellationToken);

            return CustomResponsePost($"produtos/{request.Id}", result, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarProduto([FromRoute] Guid id, ProdutoRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            if (id != request.Id)
            {
                return ErrorBadRequestPutId();
            }

            var result = await produtosController.AtualizarProdutoAsync(request, cancellationToken);

            return CustomResponsePutPatch(result, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarProduto([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await produtosController.DeletarProdutoAsync(id, cancellationToken);

            return result ? NoContent() : CustomResponseDelete(id, result);
        }
    }
}
