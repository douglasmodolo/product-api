using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 1 e 80 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(300, ErrorMessage = "A descrição deve ter entre 1 e 300 caracteres", MinimumLength = 1)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        public decimal Price { get; set; }

        public string? UrlImage { get; set; }

        public int CategoryId { get; set; }
    }
}
