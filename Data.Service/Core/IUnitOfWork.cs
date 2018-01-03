using System;
using System.Threading.Tasks;

namespace Data.Service.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ILogRepository Logs { get; }
        Task<int> Commit();
    }
}
