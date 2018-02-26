using Data.Service.Core.Entities;
using LVMiniAdminApi.Models.UserModels;

namespace LVMiniAdminApi.Contracts
{
    public interface IModifiedUserHandler
    {
        User SetChangesToStoredUser(User storedUser, BaseModifiedUserModelDto modifiedModel);
        bool CheckTheChanges(User storedUserWithTheChanges, BaseModifiedUserModelDto user);
    }
}
