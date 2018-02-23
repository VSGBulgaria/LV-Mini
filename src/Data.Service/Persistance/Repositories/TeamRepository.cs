using System.Collections.Generic;
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

        public async Task<Team> GetByTeamName(string teamName)
        {
            return await Entities
                .Include(team => team.UsersTeams)
                .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.TeamName.Equals(teamName));
        }

        public async Task<ICollection<Team>> GetAll()
        {
            return await Entities
                .AsNoTracking()
                .Include(team => team.UsersTeams)
                .ThenInclude(ut => ut.User)
                .ToListAsync();
        }

        public async Task<Team> Get(string teamName)
        {
            return await Entities
                .Include(team => team.UsersTeams)
                .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.TeamName == teamName);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
