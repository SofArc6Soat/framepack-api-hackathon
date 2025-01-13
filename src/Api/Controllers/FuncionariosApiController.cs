using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("funcionarios")]
    public class FuncionariosApiController(IFuncionarioController funcionarioController, INotificador notificador) : MainController(notificador)
    {
        [HttpPost]
        public async Task<IActionResult> CadastrarFuncionarioAsync(FuncionarioRequestDto usuarioRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await funcionarioController.CadastrarFuncionarioAsync(usuarioRequestDto, cancellationToken);

            usuarioRequestDto.Senha = "*******";

            return CustomResponsePost($"funcionarios/{usuarioRequestDto.Id}", result, result);
        }
    }
}
