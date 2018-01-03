using AuthorizationServer.Helpers;
using Data.Service.Core;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace AuthorizationServer.Configuration
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.GetByUsernameAndPassword(context.UserName,
                Hasher.PasswordHash(context.Password));

            if (user != null)
            {
                context.Result = new GrantValidationResult(user.Id.ToString(), authenticationMethod: "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
            }
        }
    }
}
