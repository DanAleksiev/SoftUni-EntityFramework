using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUsersDTO, User>();
            CreateMap<ImportProductDTO, Product>();
            CreateMap<ImportCategoriesDTO, Category>();
            CreateMap<ImportCategoryProductDTO, CategoryProduct>();
        }
    }
}
