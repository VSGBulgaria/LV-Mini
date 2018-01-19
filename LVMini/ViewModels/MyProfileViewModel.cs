using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LVMini.ViewModels
{
    public class MyProfileViewModel
    {

        
        [EmailAddress]
        public string Email { get; set; }

       
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Code { get; set; }
    }
}
