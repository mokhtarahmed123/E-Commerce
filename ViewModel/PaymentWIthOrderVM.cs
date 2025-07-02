using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModel
{
    public class PaymentWIthOrderVM
    {
        public int PaymentId { get; set; }

        [Required]
        [RegularExpression("^(Cash|Visa)$", ErrorMessage = "Payment must be Cash or Visa")]
        public string PaymentMethod { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string? ProductName { get; set; } 

    }
}
