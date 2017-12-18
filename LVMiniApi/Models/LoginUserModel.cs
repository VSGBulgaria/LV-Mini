﻿using Data.Service.Entities;
using System.ComponentModel.DataAnnotations;

namespace LVMiniApi.Models
{
    public class LoginUserModel : IUser
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
