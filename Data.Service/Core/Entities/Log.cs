using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Data.Service.Core.Entities
{
    public class Log : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int ActionId { get; set; }
        public LogAction Action { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}
