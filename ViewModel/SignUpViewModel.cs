using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Phone]

        [Required]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
       
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; } 


    }
}
