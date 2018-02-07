using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Service.Core.Entities;

namespace Data.Service.Core.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<Team> GetByTeamName(string teamName);
        Task<ICollection<Team>> GetAll();
        Task<Team> Get(string teamName); 
        Task<int> SaveChangesAsync();
    }
}
