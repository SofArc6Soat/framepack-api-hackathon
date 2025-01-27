using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Gateways.Cognito.Dtos.Request;
using Gateways.Cognito.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Framepack_WebApi.Tests.External.Api.Controllers;

public class UsuariosApiControllerTests
{
    private readonly Mock<IUsuarioController> _usuarioControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly UsuariosApiController _usuariosApiController;

    public UsuariosApiControllerTests()
    {
        _usuarioControllerMock = new Mock<IUsuarioController>();
        _notificadorMock = new Mock<INotificador>();
        _usuariosApiController = new UsuariosApiController(_usuarioControllerMock.Object, _notificadorMock.Object);
    }

    /* [Fact]
     public async Task CadastrarUsuarioAsync_Success()
     {
         // Arrange
         var usuarioRequestDto = new UsuarioRequestDto
         {
             Id = Guid.NewGuid(),
             Nome = "Teste",
             Email = "teste@teste.com",
             Senha = "Senha123!"
         };
         var cancellationToken = CancellationToken.None;

         _usuarioControllerMock
             .Setup(x => x.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken))
             .ReturnsAsync(true);

         // Act
         var result = await _usuariosApiController.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken);

         // Assert
         var actionResult = Assert.IsType<CreatedResult>(result);

         // Verificar se o valor retornado é do tipo esperado (objeto anônimo)
         dynamic value = actionResult.Value;
         Assert.NotNull(value);

         // Verificar se a propriedade "success" existe e é verdadeira
         Assert.True(value.success);

         _usuarioControllerMock.Verify(x => x.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken), Times.Once);
     }*/


    [Fact]
    public async Task CadastrarUsuarioAsync_Failure()
    {
        // Arrange
        var usuarioRequestDto = new UsuarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Teste",
            Email = "teste@teste.com",
            Senha = "Senha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioControllerMock
            .Setup(x => x.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuariosApiController.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao cadastrar usuario.", actionResult.Value);
        _usuarioControllerMock.Verify(x => x.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken), Times.Once);
    }

    /*    [Fact]
        public async Task IdentificarUsuario_Success()
        {
            // Arrange
            var identifiqueSeRequestDto = new IdentifiqueSeRequestDto
            {
                Email = "teste@teste.com",
                Senha = "Senha123!"
            };
            var tokenUsuario = new TokenUsuario
            {
                Email = "teste@teste.com",
                AccessToken = "access_token",
                RefreshToken = "refresh_token",
                Expiry = DateTimeOffset.UtcNow.AddHours(1)
            };
            var cancellationToken = CancellationToken.None;

            _usuarioControllerMock
                .Setup(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto, cancellationToken))
                .ReturnsAsync(tokenUsuario);

            // Act
            var result = await _usuariosApiController.IdentificarUsuario(identifiqueSeRequestDto, cancellationToken);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)actionResult.Value);
            _usuarioControllerMock.Verify(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto, cancellationToken), Times.Once);
        }*/

    [Fact]
    public async Task IdentificarUsuario_Failure()
    {
        // Arrange
        var identifiqueSeRequestDto = new IdentifiqueSeRequestDto
        {
            Email = "teste@teste.com",
            Senha = "Senha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioControllerMock
            .Setup(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto, cancellationToken))
            .ReturnsAsync((TokenUsuario?)null);

        // Act
        var result = await _usuariosApiController.IdentificarUsuario(identifiqueSeRequestDto, cancellationToken);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao identificar usuario.", actionResult.Value);
        _usuarioControllerMock.Verify(x => x.IdentificarUsuarioAsync(identifiqueSeRequestDto, cancellationToken), Times.Once);
    }
    /*
        [Fact]
        public async Task ConfirmarEmailVerificaoAsync_Success()
        {
            // Arrange
            var confirmarEmailVerificacaoDto = new ConfirmarEmailVerificacaoDto
            {
                Email = "teste@teste.com",
                CodigoVerificacao = "123456"
            };
            var cancellationToken = CancellationToken.None;

            _usuarioControllerMock
                .Setup(x => x.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken))
                .ReturnsAsync(true);

            // Act
            var result = await _usuariosApiController.ConfirmarEmailVerificaoAsync(confirmarEmailVerificacaoDto, cancellationToken);

            // Assert
            var actionResult = Assert.IsType<CreatedResult>(result);
            dynamic value = actionResult.Value;
            Assert.True(value.success);
            _usuarioControllerMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken), Times.Once);
        }
    */
    [Fact]
    public async Task ConfirmarEmailVerificaoAsync_Failure()
    {
        // Arrange
        var confirmarEmailVerificacaoDto = new ConfirmarEmailVerificacaoDto
        {
            Email = "teste@teste.com",
            CodigoVerificacao = "123456"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioControllerMock
            .Setup(x => x.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuariosApiController.ConfirmarEmailVerificaoAsync(confirmarEmailVerificacaoDto, cancellationToken);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao confirmar verificacao de e-mail.", actionResult.Value);
        _usuarioControllerMock.Verify(x => x.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken), Times.Once);
    }

    /* [Fact]
     public async Task SolicitarRecuperacaoSenhaAsync_Success()
     {
         // Arrange
         var solicitarRecuperacaoSenhaDto = new SolicitarRecuperacaoSenhaDto
         {
             Email = "teste@teste.com"
         };
         var cancellationToken = CancellationToken.None;

         _usuarioControllerMock
             .Setup(x => x.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken))
             .ReturnsAsync(true);

         // Act
         var result = await _usuariosApiController.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken);

         // Assert
         var actionResult = Assert.IsType<CreatedResult>(result);
         var value = actionResult.Value as IDictionary<string, object>;
         Assert.NotNull(value);
         Assert.True((bool)value["success"]);
         _usuarioControllerMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken), Times.Once);
     }*/

    [Fact]
    public async Task SolicitarRecuperacaoSenhaAsync_Failure()
    {
        // Arrange
        var solicitarRecuperacaoSenhaDto = new SolicitarRecuperacaoSenhaDto
        {
            Email = "teste@teste.com"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioControllerMock
            .Setup(x => x.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuariosApiController.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao solicitar recuperacao de senha.", actionResult.Value);
        _usuarioControllerMock.Verify(x => x.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken), Times.Once);
    }
    /*
        [Fact]
        public async Task EfetuarResetSenhaAsync_Success()
        {
            // Arrange
            var resetarSenhaDto = new ResetarSenhaDto
            {
                Email = "teste@teste.com",
                CodigoVerificacao = "123456",
                NovaSenha = "NovaSenha123!"
            };
            var cancellationToken = CancellationToken.None;

            _usuarioControllerMock
                .Setup(x => x.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken))
                .ReturnsAsync(true);

            // Act
            var result = await _usuariosApiController.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)actionResult.Value);
            _usuarioControllerMock.Verify(x => x.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken), Times.Once);
        }*/

    [Fact]
    public async Task EfetuarResetSenhaAsync_Failure()
    {
        // Arrange
        var resetarSenhaDto = new ResetarSenhaDto
        {
            Email = "teste@teste.com",
            CodigoVerificacao = "123456",
            NovaSenha = "NovaSenha123!"
        };
        var cancellationToken = CancellationToken.None;

        _usuarioControllerMock
            .Setup(x => x.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _usuariosApiController.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao resetar senha.", actionResult.Value);
        _usuarioControllerMock.Verify(x => x.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken), Times.Once);
    }
}
