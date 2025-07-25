using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebAPI.Models.Interfaces;

namespace WebAPI.Models
{
    [Table("Categories")]
    public class Category : IHasTimestamps
    {
        public Category()
        {
            Products = new Collection<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 1 e 80 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(300, ErrorMessage = "A descrição deve ter entre 1 e 300 caracteres", MinimumLength = 1)]
        public string? UrlImage { get; set; }

        public DateTime CreatedAt { get; set; }        
        public DateTime UpdatedAt { get; set; }
        
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }        
    }
}
