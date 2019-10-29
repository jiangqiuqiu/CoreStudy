using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCServer.ViewModels
{
    public class LoginViewModel
    {

        //[Required]
        ////[DataType(DataType.EmailAddress)]
        //public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
        public bool RemeberMe { get; set; }
    }
}
