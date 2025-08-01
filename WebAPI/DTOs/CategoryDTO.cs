using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 1 e 80 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(300, ErrorMessage = "A descrição deve ter entre 1 e 300 caracteres", MinimumLength = 1)]
        public string? UrlImage { get; set; }
    }
}
