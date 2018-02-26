
using System;

namespace LVMiniAdminApi.Models.UserModels
{
    public class UserDto : BaseModifiedUserModelDto, IEquatable<UserDto>
    {
        public bool Equals(UserDto other)
        {
            return this.Username.Equals(other.Username);
        }
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + this.Username.GetHashCode();
            hash = hash * 23 + this.Email.GetHashCode();
            hash = hash * 23 + this.FirstName.GetHashCode();
            hash = hash * 23 + this.LastName.GetHashCode();
            hash = hash * 23 + this.IsActive.GetHashCode();
            return hash;
        }
    }
}
