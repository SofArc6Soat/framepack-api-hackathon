using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DataAnnotations
{
    public class MaxFileSizeAttribute(long maxFileSize) : ValidationAttribute
    {
#pragma warning disable CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
#pragma warning disable CS8603 // Possível retorno de referência nula.
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) =>
            value is IFormFile file && file.Length > maxFileSize
                ? new ValidationResult(ErrorMessage ?? $"O tamanho do arquivo não deve exceder {maxFileSize} bytes.")
                : ValidationResult.Success;
#pragma warning restore CS8603 // Possível retorno de referência nula.
#pragma warning restore CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
    }
}
