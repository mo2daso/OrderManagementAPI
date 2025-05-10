using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Range(1, 5000)]
        public decimal TotalAmount { get; set; }

        [MinLength(1, ErrorMessage = "At least one product must be included.")]
        public List<Product> Products { get; set; } = new();
    }
}
