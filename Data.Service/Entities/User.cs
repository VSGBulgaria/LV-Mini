using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Service.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
    }
}
