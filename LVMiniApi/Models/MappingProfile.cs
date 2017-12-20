using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Service.Core.Entities;

namespace LVMiniApi.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(u => u.Url,
                opt => opt.ResolveUsing<UserUrlResolver>());
        }
    }
}
