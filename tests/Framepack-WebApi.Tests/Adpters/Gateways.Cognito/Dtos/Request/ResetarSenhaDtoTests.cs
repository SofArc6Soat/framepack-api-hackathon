using Gateways.Cognito.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace Framepack_WebApi.Tests.Adpters.Gateways.Cognito.Dtos.Request
{
    public class ResetarSenhaDtoTests
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
            var model = new ResetarSenhaDto { Email = null, CodigoVerificacao = "123456", NovaSenha = "NovaSenha123" };
            var results = ValidateModel(model);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(ResetarSenhaDto.Email)));
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new ResetarSenhaDto { Email = "invalid-email", CodigoVerificacao = "123456", NovaSenha = "NovaSenha123" };
            var results = ValidateModel(model);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(ResetarSenhaDto.Email)));
        }

        [Fact]
        public void Should_Have_Error_When_CodigoVerificacao_Is_Not_6_Characters()
        {
            var model = new ResetarSenhaDto { Email = "test@example.com", CodigoVerificacao = "12345", NovaSenha = "NovaSenha123" };
            var results = ValidateModel(model);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(ResetarSenhaDto.CodigoVerificacao)));
        }

        [Fact]
        public void Should_Have_Error_When_NovaSenha_Is_Too_Short()
        {
            var model = new ResetarSenhaDto { Email = "test@example.com", CodigoVerificacao = "123456", NovaSenha = "short" };
            var results = ValidateModel(model);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(ResetarSenhaDto.NovaSenha)));
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new ResetarSenhaDto { Email = "test@example.com", CodigoVerificacao = "123456", NovaSenha = "NovaSenha123" };
            var results = ValidateModel(model);
            Assert.Empty(results);
        }
    }
}