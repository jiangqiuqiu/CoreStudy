using System.ComponentModel.DataAnnotations;

namespace OIDCServer.ViewModels
{
    public class RegisterViewModel
    {
        //[Required]
        ////[DataType(DataType.EmailAddress)]
        //public string UserName{get;set;}

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password{get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmedPassword{get;set;}

    }   
}
