using System.Collections.Generic;

namespace LVMini.Models
{
    public class Team
    {
        public string TeamName { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ModifiedUserModel> Users { get; set; }
    }
}
