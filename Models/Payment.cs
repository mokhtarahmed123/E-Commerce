using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("Cash | Visa",ErrorMessage ="Payment must be Cash Or Visa")]
        public string PaymentMethod { get; set; }

        [ForeignKey("Order")]
        public int OrderID {  get; set; }
        public OrderDetailes Order { get; set; }

    }
}
