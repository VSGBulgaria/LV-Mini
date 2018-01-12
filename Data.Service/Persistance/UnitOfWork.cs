using Data.Service.Core.Interfaces;
using System.Threading.Tasks;

namespace Data.Service.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LvMiniDbContext _context;

        public UnitOfWork(LvMiniDbContext context, IUserRepository users, ILogRepository logs)
        {
            _context = context;
            Users = users;
            Logs = logs;
        }


        public IUserRepository Users { get; }
        public ILogRepository Logs { get; }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
