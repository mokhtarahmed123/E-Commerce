using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel
{
    public class OrderWithCustomerAndProduct
    {
        #region Order
        public int OredrId { get; set; }

        public decimal TotalPrice { get; set; }
        #endregion
        #region Product
        public int ProductId { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name Must Be Greater Than 3")]
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }
        //[Required]

        [DataType(DataType.ImageUrl)]
        public string? UrlImage { get; set; }

        #endregion
        #region Customer
        public int CustomerId { get; set; }
        //[Required(ErrorMessage = "Please Enter Your Name")]
        [StringLength(maximumLength: 3, ErrorMessage = "Name Must Be Greater than 3")]
        [Display(Name = " Name ")]
        public string? CustomerName { get; set; }
        //[Required(ErrorMessage = "Phone Is Required")]
        [DataType(DataType.PhoneNumber)]
        public string? CustomerPhoneNumber { get; set; }
     

        #endregion
      
    }
}
