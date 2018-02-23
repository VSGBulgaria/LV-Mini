using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("Teams", Schema = "admin")]
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string TeamName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<UserTeam> UsersTeams { get; set; } = new List<UserTeam>();
    }
}
