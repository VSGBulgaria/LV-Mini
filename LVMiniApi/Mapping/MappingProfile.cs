using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniApi.Models;

namespace LVMiniApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //From Entity to Model
            CreateMap<User, UserModel>()
                .ForMember(u => u.Url,
                    opt => opt.ResolveUsing<UserUrlResolver>())
                .ReverseMap();

            CreateMap<User, EditUserModel>()
                .ReverseMap();

            //From Model to Entity
        }
    }
}
