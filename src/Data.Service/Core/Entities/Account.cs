﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Service.Core.Entities
{
    [Table("Account", Schema = "IbClue")]
    public class Account
    {
        [Key]
        public int IDAccount { get; set; }

        public int? IdAccountSource { get; set; }

        [MaxLength(100)]
        public string AccountNumber { get; set; }

        [MaxLength(15)]
        public string ProductCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string AccountCategoryCode { get; set; }

        [MaxLength(10)]
        public string AccountStatusCode { get; set; }
    }
}
