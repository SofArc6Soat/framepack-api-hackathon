using Core.Domain.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Dtos.Request
{
    public record ClienteAtualizarRequestDto
    {
        [RequiredGuid(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required bool Ativo { get; set; }
    }
}
