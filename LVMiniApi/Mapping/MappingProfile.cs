using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniApi.Models;

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
                .ForMember(u => u.Name,
                    opt => opt.MapFrom(u => $"{u.FirstName} {u.LastName}"))
                .ReverseMap();

            CreateMap<RegisterUserDto, User>();

            CreateMap<User, EditUserDto>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition(
                    (dto, user, dtoMember, userMember) => dtoMember != null));
        }
    }
}
