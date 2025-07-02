using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Name Must Be Greater Than 3")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        [Range(1,1000)]
        public int? Quantity{ get; set; }
        [Required]
        
        [DataType(DataType.ImageUrl)]
        public string? UrlImage {  get; set; }
        [Range(1, 100,ErrorMessage = " CustomerId Must be Greater than 0")]
        [ForeignKey("Customers")]
        public int? CustomerId {  get; set; }
        public Customer Customers { get; set; }   
        [ForeignKey("category")]
        [Range(1,100)]
        public int? CategoryId {  get; set; }
        
        public Category? category { get; set; }

        List<OrderDetailes>? Detailes { get; set; }


    }
}
