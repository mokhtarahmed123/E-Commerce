using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey( "UserApp")]
        public string UserAppId { get; set; }
        public UserApp UserApp { get; set; }
        [Required(ErrorMessage = "Please Enter Your Name")]
        [Display(Name = " Name ")]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public List<Product>? products { get; set; }
        public List<OrderDetailes>? orders { get; set; }
    }
}
