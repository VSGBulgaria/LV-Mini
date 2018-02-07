using Data.Service.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Data.Service.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private LvMiniDbContext _context;


        public UnitOfWork(LvMiniDbContext context, IUserRepository userRepository, ILogRepository logRepository, IProductGroupRepository productGroupRepository)
        {
            _context = context;
            UserRepository = userRepository;
            LogRepository = logRepository;
            ProductGroupRepository = productGroupRepository;

        }

        public IUserRepository UserRepository { get; }
        public ILogRepository LogRepository { get; }
        public IProductGroupRepository ProductGroupRepository { get; }

        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
