using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;

namespace Data.Service.Persistance.Repositories
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        public LogRepository(LvMiniDbContext context)
            : base(context)
        {
        }
    }
}
