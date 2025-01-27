using Gateways.Cognito.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito.Dtos.Request;

public class IdentifiqueSeRequestDtoTests
{
    private ValidationContext CreateContext(object model) => new(model, null, null);

    private IList<ValidationResult> ValidateModel(object model)
    {
        var context = CreateContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Null()
    {
        var model = new IdentifiqueSeRequestDto { Email = null, Senha = "SenhaSegura123" };
        var results = ValidateModel(model);
        Assert.Contains(results, v => v.MemberNames.Contains(nameof(IdentifiqueSeRequestDto.Email)));
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new IdentifiqueSeRequestDto { Email = "invalid-email", Senha = "SenhaSegura123" };
        var results = ValidateModel(model);
        Assert.Contains(results, v => v.MemberNames.Contains(nameof(IdentifiqueSeRequestDto.Email)));
    }

    [Fact]
    public void Should_Have_Error_When_Senha_Is_Too_Short()
    {
        var model = new IdentifiqueSeRequestDto { Email = "test@example.com", Senha = "short" };
        var results = ValidateModel(model);
        Assert.Contains(results, v => v.MemberNames.Contains(nameof(IdentifiqueSeRequestDto.Senha)));
    }

    [Fact]
    public void Should_Have_Error_When_Senha_Is_Too_Long()
    {
        var model = new IdentifiqueSeRequestDto { Email = "test@example.com", Senha = new string('a', 51) };
        var results = ValidateModel(model);
        Assert.Contains(results, v => v.MemberNames.Contains(nameof(IdentifiqueSeRequestDto.Senha)));
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new IdentifiqueSeRequestDto { Email = "test@example.com", Senha = "SenhaSegura123" };
        var results = ValidateModel(model);
        Assert.Empty(results);
    }
}