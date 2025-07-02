using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class OrderDetailes
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
      

        public decimal TotalPrice { get; set; }

        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }
        public Customer Customers { get; set; }
        public List<Payment>? payments { get; set; }

    }
}
