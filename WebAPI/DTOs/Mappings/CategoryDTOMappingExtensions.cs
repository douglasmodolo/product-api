
using WebAPI.Models;

namespace WebAPI.DTOs.Mappings
{
    public static class CategoryDTOMappingExtensions
    {
        public static CategoryDTO? ToCategoryDTO(this Category category)
        {
            if (category == null) return null;

            var categoryDto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlImage = category.UrlImage
            };

            return categoryDto;
        }

        public static Category? ToCategory(this CategoryDTO categoryDTO)
        {
            if (categoryDTO == null) return null;

            var category = new Category
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
                UrlImage = categoryDTO.UrlImage
            };

            return category;
        }

        public static IEnumerable<CategoryDTO> ToCategoryDTOList(this IEnumerable<Category> categories)
        {
            if (categories == null || !categories.Any()) return new List<CategoryDTO>();

            var categoryDtos = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                UrlImage = c.UrlImage
            }).ToList();

            return categoryDtos;
        }
    }
}
