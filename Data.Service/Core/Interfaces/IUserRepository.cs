using Data.Service.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        IEnumerable<User> GetAll(int pageNumber, int pageSize);
        Task<User> GetByUsername(string username);
        Task<User> GetBySubjectId(string subjectId);
        Task<User> GetByEmail(string email);
        Task<User> GetByProvider(string loginProvider, string providerKey);
        Task<IEnumerable<UserLogin>> GetUserLogin(string subjectId);
        Task<IEnumerable<UserClaim>> GetUserClaims(string subjectId);
        Task<bool> UserExists(string username);
        Task<bool> AreUserCredentialsValid(string username, string password);
        Task<bool> IsUserActive(string subjectId);
        Task AddUserLogin(string subjectId, string loginProvider, string providerKey);
        Task AddUserClaim(string subjectId, string claimType, string claimValue);
    }
}
