using AutoMapper;
using WebAPI.Models;

namespace WebAPI.DTOs.Mappings
{
    public class ProductDTOMappingProfile : Profile
    {
        public ProductDTOMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductRequestDto>().ReverseMap();
            CreateMap<Product, UpdateProductResponseDto>().ReverseMap();
        }
    }
}
