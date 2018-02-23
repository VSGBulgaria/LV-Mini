using Data.Service.Core.Interfaces;
using LVMiniAdminApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniAdminApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IUserRepository _userRepository;
        protected IModifiedUserHandler _userHandler;
        protected ITeamRepository _teamRepository;
    }
}