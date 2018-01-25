using System;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ILogRepository Logs { get; }
        Task<bool> Commit();
    }
}
