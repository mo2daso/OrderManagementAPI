using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }

        public string? Category { get; set; }
    }
}

