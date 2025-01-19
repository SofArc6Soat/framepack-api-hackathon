using Gateways.Cognito.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito.Dtos.Request;

public class SolicitarRecuperacaoSenhaDtoTests
{
    [Theory]
    [InlineData("test@example.com", true)] // Caso válido
    [InlineData("", false)] // E-mail vazio
    [InlineData("invalid-email", false)] // E-mail inválido
    [InlineData("a@b.co", true)] // E-mail com o mínimo de caracteres válidos
    public void SolicitarRecuperacaoSenhaDto_ValidacaoDeveRetornarResultadoCorreto(string email, bool isValid)
    {
        // Arrange
        var dto = new SolicitarRecuperacaoSenhaDto { Email = email };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
        var result = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.Equal(isValid, result);
    }


    [Fact]
    public void SolicitarRecuperacaoSenhaDto_EmailComMaisDe100Caracteres_DeveRetornarInvalido()
    {
        // Arrange
        var longEmail = new string('a', 101) + "@example.com"; // E-mail com mais de 100 caracteres
        var dto = new SolicitarRecuperacaoSenhaDto { Email = longEmail };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
        var result = Validator.TryValidateObject(dto, validationContext, validationResults, true);

        // Assert
        Assert.False(result);
        Assert.Contains(validationResults, v => v.ErrorMessage == "O campo E-mail deve conter entre 5 e 100 caracteres.");
    }
}