using System.Linq;
using System.Threading.Tasks;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Persistance.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(LvMiniDbContext context) 
            : base(context)
        {
        }

        public async Task<Team> GetByTeamName(string username)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TeamName.Equals(username));
        }
    }
}
