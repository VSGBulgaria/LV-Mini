using Microsoft.AspNetCore.Authorization;

namespace LVMiniApi.Authorization
{
    public class MustBeLoggedInUserRequirement : IAuthorizationRequirement
    {
    }
}
