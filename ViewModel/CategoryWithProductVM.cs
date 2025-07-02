using E_Commerce.Custom_Attribute;
using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel
{
    public class CategoryWithProductVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [Unique]
        [MinLength(2, ErrorMessage = "Name Must Be Greater Than 2")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
