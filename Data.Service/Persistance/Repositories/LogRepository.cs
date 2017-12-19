using System;
using System.Collections.Generic;
using System.Text;
using Data.Service.Core;
using Data.Service.Core.Entities;

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
