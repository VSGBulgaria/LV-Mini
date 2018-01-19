using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace LVMiniApi.Authorization
{
    public class MustBeLoggedInUserHandler : AuthorizationHandler<MustBeLoggedInUserRequirement>
    {
        private readonly IUserRepository _userRepository;

        public MustBeLoggedInUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, MustBeLoggedInUserRequirement requirement)
        {
            if (!(context.Resource is AuthorizationFilterContext filterContext))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            var userId = filterContext.RouteData.Values["id"].ToString();
            var ownerId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (!_userRepository.UserIsOwner(ownerId, userId))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }
}
