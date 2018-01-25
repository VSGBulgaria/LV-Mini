using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Service.Persistance.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LvMiniDbContext context)
            : base(context)
        {
        }

        public async Task<User> GetByUsername(string username)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetBySubjectId(string subjectId)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.SubjectId == subjectId);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Claims.Any(c => c.ClaimType == "email" && c.ClaimValue == email));
        }

        public async Task<User> GetByProvider(string loginProvider, string providerKey)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
        }

        public async Task<IEnumerable<UserClaim>> GetUserClaims(string subjectId)
        {
            // get user with claims
            User user = await Entities.Include("Claims").FirstOrDefaultAsync(u => u.SubjectId == subjectId);
            if (user == null)
            {
                return new List<UserClaim>();
            }
            return user.Claims.ToList();
        }

        public async Task<IEnumerable<UserLogin>> GetUserLogin(string subjectId)
        {
            User user = await Entities.Include("Logins").FirstOrDefaultAsync(u => u.SubjectId == subjectId);
            if (user == null)
            {
                return new List<UserLogin>();
            }
            return user.Logins.ToList();
        }

        public async Task<bool> AreUserCredentialsValid(string username, string password)
        {
            // get the user
            User user = await GetByUsername(username);
            if (user == null)
            {
                return false;
            }
            return user.Password == password && !string.IsNullOrWhiteSpace(password);
        }

        public async Task<bool> IsUserActive(string subjectId)
        {
            User user = await GetBySubjectId(subjectId);
            return user.IsActive;
        }

        public async Task AddUserLogin(string subjectId, string loginProvider, string providerKey)
        {
            User user = await GetBySubjectId(subjectId);

            if (user == null)
            {
                throw new ArgumentException("User with given username not found!", subjectId);
            }
            user.Logins.Add(new UserLogin()
            {
                SubjectId = subjectId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            });
        }

        public async Task AddUserClaim(string subjectId, string claimType, string claimValue)
        {
            User user = await GetBySubjectId(subjectId);
            if (user == null)
            {
                throw new ArgumentException("User with given username not found!", subjectId);
            }

            user.Claims.Add(new UserClaim(claimType, claimValue));
        }
    }
}
