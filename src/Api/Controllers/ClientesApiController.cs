using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "AdminRole")]
    [Route("clientes")]
    public class ClientesApiController(IClienteController clienteController, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosClientes(CancellationToken cancellationToken)
        {
            var result = await clienteController.ObterTodosClientesAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await clienteController.CadastrarClienteAsync(clienteRequestDto, cancellationToken);

            clienteRequestDto.Senha = "*******";

            return CustomResponsePost($"clientes/{clienteRequestDto.Id}", result, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarCliente([FromRoute] Guid id, ClienteAtualizarRequestDto clienteAtualizarRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            if (id != clienteAtualizarRequestDto.Id)
            {
                return ErrorBadRequestPutId();
            }

            var result = await clienteController.AtualizarClienteAsync(clienteAtualizarRequestDto, cancellationToken);

            return CustomResponsePutPatch(result, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarCliente([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await clienteController.DeletarClienteAsync(id, cancellationToken);

            return CustomResponseDelete(id, result);
        }
    }
}
