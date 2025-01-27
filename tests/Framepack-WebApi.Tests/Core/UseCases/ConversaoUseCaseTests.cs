﻿using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;

using Gateways;
using Moq;
using UseCases;

namespace Framepack_WebApi.Tests.Core.UseCases;

public class ConversaoUseCaseTests
{
    private readonly Mock<IConversaoGateway> _conversaoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly ConversaoUseCase _conversaoUseCase;

    public ConversaoUseCaseTests()
    {
        _conversaoGatewayMock = new Mock<IConversaoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _conversaoUseCase = new ConversaoUseCase(_conversaoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Success()
    {
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Status.AguardandoConversao, "video.mp4", null);
        _conversaoGatewayMock.Setup(g => g.EfetuarUploadAsync(conversao, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, CancellationToken.None);

        Assert.True(result);
        _notificadorMock.Verify(n => n.Handle(It.IsAny<Notificacao>()), Times.Never);
    }

    [Fact]
    public async Task EfetuarUploadAsync_Failure()
    {
        var conversao = new Conversao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, Status.AguardandoConversao, "video.mp4", null);
        _conversaoGatewayMock.Setup(g => g.EfetuarUploadAsync(conversao, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _conversaoUseCase.EfetuarUploadAsync(conversao, CancellationToken.None);

        Assert.False(result);
        _notificadorMock.Verify(n => n.Handle(It.Is<Notificacao>(n => n.Mensagem == "Ocorreu um erro ao efetuar o upload do vídeo.")), Times.Once);
    }

    [Fact]
    public async Task ObterConversoesPorUsuarioAsync_Success()
    {
        var usuarioId = Guid.NewGuid();
        var conversoes = new List<Conversao> { new Conversao(Guid.NewGuid(), usuarioId, DateTime.Now, Status.Concluido, "video.mp4", null) };
        _conversaoGatewayMock.Setup(g => g.ObterConversoesPorUsuarioAsync(usuarioId, It.IsAny<CancellationToken>())).ReturnsAsync(conversoes);

        var result = await _conversaoUseCase.ObterConversoesPorUsuarioAsync(usuarioId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_Success()
    {
        var usuarioId = Guid.NewGuid();
        var conversaoId = Guid.NewGuid();
        var conversao = new Conversao(conversaoId, usuarioId, DateTime.Now, Status.Concluido, "video.mp4", "urlVideo", "urlCompactado");
        var arquivo = new Arquivo(new byte[] { 1, 2, 3 }, "video.mp4");

        _conversaoGatewayMock.Setup(g => g.ObterConversaoAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>())).ReturnsAsync(conversao);
        _conversaoGatewayMock.Setup(g => g.EfetuarDownloadAsync(conversao, It.IsAny<CancellationToken>())).ReturnsAsync(arquivo);

        var result = await _conversaoUseCase.EfetuarDownloadAsync(usuarioId, conversaoId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(arquivo, result);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_ConversaoInexistente()
    {
        var usuarioId = Guid.NewGuid();
        var conversaoId = Guid.NewGuid();

        _conversaoGatewayMock.Setup(g => g.ObterConversaoAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>())).ReturnsAsync((Conversao)null);

        var result = await _conversaoUseCase.EfetuarDownloadAsync(usuarioId, conversaoId, CancellationToken.None);

        Assert.Null(result);
        _notificadorMock.Verify(n => n.Handle(It.Is<Notificacao>(n => n.Mensagem == "Conversao Inexistente")), Times.Once);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_ArquivoCompactadoNaoDisponivel()
    {
        var usuarioId = Guid.NewGuid();
        var conversaoId = Guid.NewGuid();
        var conversao = new Conversao(conversaoId, usuarioId, DateTime.Now, Status.Concluido, "video.mp4", "urlVideo", null);

        _conversaoGatewayMock.Setup(g => g.ObterConversaoAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>())).ReturnsAsync(conversao);

        var result = await _conversaoUseCase.EfetuarDownloadAsync(usuarioId, conversaoId, CancellationToken.None);

        Assert.Null(result);
        _notificadorMock.Verify(n => n.Handle(It.Is<Notificacao>(n => n.Mensagem == "O arquivo compactado ainda não está disponível para download")), Times.Once);
    }

    [Fact]
    public async Task EfetuarDownloadAsync_FalhaNoDownload()
    {
        var usuarioId = Guid.NewGuid();
        var conversaoId = Guid.NewGuid();
        var conversao = new Conversao(conversaoId, usuarioId, DateTime.Now, Status.Concluido, "video.mp4", "urlVideo", "urlCompactado");

        _conversaoGatewayMock.Setup(g => g.ObterConversaoAsync(usuarioId, conversaoId, It.IsAny<CancellationToken>())).ReturnsAsync(conversao);
        _conversaoGatewayMock.Setup(g => g.EfetuarDownloadAsync(conversao, It.IsAny<CancellationToken>())).ReturnsAsync((Arquivo)null);

        var result = await _conversaoUseCase.EfetuarDownloadAsync(usuarioId, conversaoId, CancellationToken.None);

        Assert.Null(result);
        _notificadorMock.Verify(n => n.Handle(It.Is<Notificacao>(n => n.Mensagem == "Falha ao efetuar o download")), Times.Once);
    }
}
