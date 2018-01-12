using AuthorizationServer.Helpers;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationServer.Configuration
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAndPassword(username, Hasher.PasswordHash(password));

            return user != null;
        }

        public Task<User> FindByUsernameAsync(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public Task<User> FindByExternalProviderAsync(string provider, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> AutoProvisionUserAsync(string provider, string userId, IEnumerable<Claim> claims)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IUserValidator
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByExternalProviderAsync(string provider, string userId);
        Task<User> AutoProvisionUserAsync(string provider, string userId, IEnumerable<Claim> claims);
    }
}
