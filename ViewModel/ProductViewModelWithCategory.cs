using E_Commerce.Custom_Attribute;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel
{
    public class ProductViewModelWithCategory
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name Must Be Greater Than 3")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [Range(10,500000,ErrorMessage ="Price Between 10 to 500000")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }
        [Required]

        [DataType(DataType.ImageUrl)]
        public string UrlImage { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category.")]
        public int? CId { get; set; }
        public List<Category> categories { get; set; } 
    }
}

