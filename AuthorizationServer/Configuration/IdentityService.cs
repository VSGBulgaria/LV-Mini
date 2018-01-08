using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace AuthorizationServer.Configuration
{
    public class IdentityService : IProfileService
    {
        private IUserRepository _userRepository;

        public IdentityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.Add(new Claim("role", "user"));


            var issuerClaims = context.Subject.Claims.ToList();
            string clientName = null;

            foreach (var issuerClaim in issuerClaims)
            {
                if (issuerClaim.Type.Equals("name"))
                {
                    clientName = issuerClaim.Value;
                    break;
                }

            }
            if (!clientName.IsNullOrEmpty())
            {
                Task<User> clientByUsername = GetAsincClientByUsername(clientName);
                context.IssuedClaims.Add(new Claim("role", clientByUsername.Result.Role));
            }
            
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(true);
        }

        private async Task<User> GetAsincClientByUsername(string clientName)
        {

            Task<User> clientByUsername = _userRepository.GetByUsername(clientName);
            return await clientByUsername;
        }
    }
}
