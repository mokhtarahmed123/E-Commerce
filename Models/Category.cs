using E_Commerce.Custom_Attribute;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Category Name")]
        [Unique]
        [MinLength(2,ErrorMessage ="Name Must Be Greater Than 2")]
        public string Name { get; set; }
        List<Product> Products { get; set; }
    }
}
