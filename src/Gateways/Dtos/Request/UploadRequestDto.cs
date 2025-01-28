using Core.Domain.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Dtos.Request
{
    public record UploadRequestDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "Nome do Arquivo à ser salvo")]
        public string NomeArquivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [AllowedExtensions([".mp4"])]
        [MaxFileSizeAttribute(500 * 1024 * 1024)] // 500 MB
        [Display(Name = "Arquivo de Vídeo")]
        public required IFormFile ArquivoVideo { get; set; }
    }
}