namespace WebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UrlImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
