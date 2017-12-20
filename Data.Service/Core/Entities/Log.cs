using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Service.Core.Enums;

namespace Data.Service.Core.Entities
{
    public class Log : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public LogAction Action { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
    }
}
