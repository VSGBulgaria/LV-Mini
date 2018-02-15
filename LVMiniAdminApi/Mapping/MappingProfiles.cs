using System.Linq;
using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniAdminApi.Models.TeamModels;
using LVMiniAdminApi.Models.UserModels;

namespace LVMiniAdminApi.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();

            CreateMap<Team, TeamDto>()
                .ForMember(team => team.Users, opt => opt.MapFrom(us => us.UsersTeams.Select(u => u.User).ToList()));


        }
    }
}
