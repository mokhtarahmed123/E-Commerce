using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
        public class UserApp:IdentityUser
    {
       

       public List<Customer> customers;

    }
}
