using E_Commerce.DB;
using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Custom_Attribute
{
    public class UniqueAttribute : ValidationAttribute
    {
      
        

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (E_CommerceContext)validationContext.GetService(typeof(E_CommerceContext));

            if (value == null)
                return null;
            string Name = value.ToString();
            Category category = context.Categorys.FirstOrDefault(c => c.Name == Name);
            if (category != null)
            {
                return new ValidationResult(errorMessage: "This Name Is Found");
            }
            return ValidationResult.Success;



        }
    }
}
