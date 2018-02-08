using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniApi.Models;
using System.Linq;

namespace LVMiniApi.Mapping
{
    /// <summary>
    /// Automapper configuration.
    /// </summary>
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //From Entity to Model and reverse.
            CreateMap<User, UserDto>()
                .ForMember(u => u.Url,
                    opt => opt.ResolveUsing<UserUrlResolver>())
                .ReverseMap();

            CreateMap<RegisterUserDto, User>();

            CreateMap<User, EditUserDto>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition(
                    (dto, user, dtoMember, userMember) => dtoMember != null));

            CreateMap<CreateProductGroupDto, ProductGroup>()
                .ForMember(pg => pg.Products,
                    opt => opt.MapFrom(pgd => pgd.Products.Select(id => new ProductGroupProduct() { IDProduct = id })))
                .ReverseMap();

            CreateMap<ProductGroup, ProductGroupDto>()
                .ForMember(pgd => pgd.Products, opt => opt.MapFrom(pg => pg.Products.Select(p => p.Product)))
                .ForMember(pg => pg.Url,
                    opt => opt.ResolveUsing<ProductGroupUrlResolver>());

            CreateMap<Product, ProductDto>();
            CreateMap<UpdateProductGroupDto, ProductGroup>()
                .ForAllMembers(opt => opt.Condition(
                    (dto, user, dtoMember, userMember) => dtoMember != null));
        }
    }
}
