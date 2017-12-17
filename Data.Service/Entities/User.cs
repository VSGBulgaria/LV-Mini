using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace Data.Service.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : BaseEntity
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Firstname")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Lastname")]
        public string Lastname { get; set; }
    }
}
