using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}
