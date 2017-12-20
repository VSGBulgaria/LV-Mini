using System.Threading.Tasks;
using Data.Service.Core.Entities;

namespace Data.Service.Core
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsername(string username);
    }
}
