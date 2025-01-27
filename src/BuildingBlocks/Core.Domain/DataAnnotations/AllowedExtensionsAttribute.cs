using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DataAnnotations
{
    public class AllowedExtensionsAttribute(string[] extensions) : ValidationAttribute
    {
#pragma warning disable CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
#pragma warning restore CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!extensions.Contains(extension))
                {
                    return new ValidationResult($"O arquivo deve ter uma das seguintes extensões: {string.Join(", ", extensions)}.");
                }
            }

#pragma warning disable CS8603 // Possível retorno de referência nula.
            return ValidationResult.Success;
#pragma warning restore CS8603 // Possível retorno de referência nula.
        }
    }
}
