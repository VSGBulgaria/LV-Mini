using Data.Service.Core.Entities;
using LVMiniAdminApi.Models;

namespace LVMiniAdminApi.Contracts
{
    public interface IModifiedUserHandler
    {
        User SetChangesToStoredUser(User storedUser, BaseModifiedUserModel modifiedModel);
        bool CheckTheChanges(User storedUserWithTheChanges, BaseModifiedUserModel user);
    }
}
