using Gateways.Cognito.Dtos.Request;

namespace SmokeTests.FakeDataFactory;

public static class UsuarioFakeDataFactory
{
    public static UsuarioRequestDto CriarUsuarioValido()
    {
        return new UsuarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Usuário Válido",
            Email = "usuario.valido@example.com",
            Senha = "SenhaValida123"
        };
    }

    public static UsuarioRequestDto CriarUsuarioComEmailInvalido()
    {
        return new UsuarioRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Usuário Inválido",
            Email = "email-invalido",
            Senha = "SenhaValida123"
        };
    }

    public static IdentifiqueSeRequestDto CriarIdentifiqueSeRequestValido()
    {
        return new IdentifiqueSeRequestDto
        {
            Email = "usuario.valido@example.com",
            Senha = "SenhaValida123"
        };
    }

    public static IdentifiqueSeRequestDto CriarIdentifiqueSeRequestInvalido()
    {
        return new IdentifiqueSeRequestDto
        {
            Email = "email-invalido",
            Senha = "SenhaInvalida"
        };
    }

    public static ConfirmarEmailVerificacaoDto CriarConfirmarEmailVerificacaoDtoValido()
    {
        return new ConfirmarEmailVerificacaoDto
        {
            Email = "usuario.valido@example.com",
            CodigoVerificacao = "123456"
        };
    }

    public static ConfirmarEmailVerificacaoDto CriarConfirmarEmailVerificacaoDtoInvalido()
    {
        return new ConfirmarEmailVerificacaoDto
        {
            Email = "email-invalido",
            CodigoVerificacao = "123"
        };
    }

    public static SolicitarRecuperacaoSenhaDto CriarSolicitarRecuperacaoSenhaDtoValido()
    {
        return new SolicitarRecuperacaoSenhaDto
        {
            Email = "usuario.valido@example.com"
        };
    }

    public static SolicitarRecuperacaoSenhaDto CriarSolicitarRecuperacaoSenhaDtoInvalido()
    {
        return new SolicitarRecuperacaoSenhaDto
        {
            Email = "email-invalido"
        };
    }

    public static ResetarSenhaDto CriarResetarSenhaDtoValido()
    {
        return new ResetarSenhaDto
        {
            Email = "usuario.valido@example.com",
            CodigoVerificacao = "123456",
            NovaSenha = "NovaSenhaValida123"
        };
    }

    public static ResetarSenhaDto CriarResetarSenhaDtoInvalido()
    {
        return new ResetarSenhaDto
        {
            Email = "email-invalido",
            CodigoVerificacao = "123",
            NovaSenha = "123"
        };
    }
}