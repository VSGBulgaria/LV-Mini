using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using Data.Service.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Data.Service.Persistance.Repositories
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        public LogRepository(LvMiniDbContext context) : base(context)
        {
        }

        public async Task InsertLog(string username, UserAction action)
        {
            string actionName = action.ToString();

            Log log = new Log
            {
                Action = actionName,
                Username = username,
                Time = DateTime.Now
            };

            await Insert(log);
        }
    }
}
