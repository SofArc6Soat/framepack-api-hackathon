using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Dtos.Request
{
    public record ClienteIdentifiqueSeRequestDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Length(11, 11, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        [Display(Name = "CPF")]
        public required string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Length(8, 50, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        [PasswordPropertyText]
        public required string Senha { get; set; }
    }

    public record FuncinarioIdentifiqueSeRequestDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "{0} está em um formato inválido.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "E-mail")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Length(8, 50, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        [PasswordPropertyText]
        public required string Senha { get; set; }
    }

}
