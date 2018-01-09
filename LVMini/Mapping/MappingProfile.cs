using AutoMapper;
using LVMini.Models;
using LVMini.ViewModels;

namespace LVMini.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, UserModel>();
        }
    }
}
