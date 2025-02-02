using Gateways.Cognito.Dtos.Request;

namespace SmokeTests.FakeDataFactory;

public static class UsuarioFakeDataFactory
{
    public static UsuarioRequestDto CriarUsuarioValido() => new()
    {
        Id = Guid.NewGuid(),
        Nome = "Usuário Válido",
        Email = "usuario.valido@example.com",
        Senha = "SenhaValida123"
    };

    public static UsuarioRequestDto CriarUsuarioComEmailInvalido() => new()
    {
        Id = Guid.NewGuid(),
        Nome = "Usuário Inválido",
        Email = "email-invalido",
        Senha = "SenhaValida123"
    };

    public static IdentifiqueSeRequestDto CriarIdentifiqueSeRequestValido() => new()
    {
        Email = "usuario.valido@example.com",
        Senha = "SenhaValida123"
    };

    public static IdentifiqueSeRequestDto CriarIdentifiqueSeRequestInvalido() => new()
    {
        Email = "email-invalido",
        Senha = "SenhaInvalida"
    };

    public static ConfirmarEmailVerificacaoDto CriarConfirmarEmailVerificacaoDtoValido() => new()
    {
        Email = "usuario.valido@example.com",
        CodigoVerificacao = "123456"
    };

    public static ConfirmarEmailVerificacaoDto CriarConfirmarEmailVerificacaoDtoInvalido() => new()
    {
        Email = "email-invalido",
        CodigoVerificacao = "123"
    };

    public static SolicitarRecuperacaoSenhaDto CriarSolicitarRecuperacaoSenhaDtoValido() => new()
    {
        Email = "usuario.valido@example.com"
    };

    public static SolicitarRecuperacaoSenhaDto CriarSolicitarRecuperacaoSenhaDtoInvalido() => new()
    {
        Email = "email-invalido"
    };

    public static ResetarSenhaDto CriarResetarSenhaDtoValido() => new()
    {
        Email = "usuario.valido@example.com",
        CodigoVerificacao = "123456",
        NovaSenha = "NovaSenhaValida123"
    };

    public static ResetarSenhaDto CriarResetarSenhaDtoInvalido() => new()
    {
        Email = "email-invalido",
        CodigoVerificacao = "123",
        NovaSenha = "123"
    };
}