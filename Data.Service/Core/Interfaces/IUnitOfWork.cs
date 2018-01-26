using System;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ILogRepository LogRepository { get; }
        Task<bool> Commit();
    }
}
