using AutoMapper;
using Matgr.Products.Application.Responses;
using Matgr.Products.Core.Entities;

namespace Matgr.Products.Application.Mappers
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

        }
    }
}
