using Data.Service.Core.Entities;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> GetByUsernameAndPassword(string username, string password);
        bool UserIsOwner(string ownerId, string dbUserId);
    }
}
