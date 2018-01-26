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
            CreateMap<User, UserModel>()
                .ForMember(u => u.Url,
                    opt => opt.ResolveUsing<UserUrlResolver>())
                .ReverseMap();

            CreateMap<User, EditUserModel>()
                .ReverseMap();
        }
    }
}
