using Core.Domain.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Dtos.Request
{
    public record FuncionarioRequestDto
    {
        [RequiredGuid(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "{0} está em um formato inválido.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "E-mail")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 8)]
        [PasswordPropertyText]
        public required string Senha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required bool Ativo { get; set; }
    }
}
