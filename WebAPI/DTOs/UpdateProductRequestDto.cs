using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class UpdateProductRequestDto : IValidatableObject
    {
        public float Stock { get; set; }
        
        [Range(0.01, 9999.99, ErrorMessage = "O preço deve estar entre 0.01 e 9999.99")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Stock < 0)
            {
                yield return new ValidationResult("O estoque não pode ser negativo", new[] { nameof(Stock) });
            }
        }
    }
}
