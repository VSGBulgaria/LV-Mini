using Data.Service.Core.Entities;
using Data.Service.Core.Enums;
using System.Threading.Tasks;

namespace Data.Service.Core.Interfaces
{
    public interface ILogRepository : IBaseRepository<Log>
    {
        Task InsertLog(string username, UserAction action);
    }
}
