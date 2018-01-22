using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Service.Core.Entities
{
    public class Log : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}
