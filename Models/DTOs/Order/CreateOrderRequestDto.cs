using System.ComponentModel.DataAnnotations;

namespace SmokingQuitSupportAPI.Models.DTOs.Order
{
    public class CreateOrderRequestDto
    {
        [Required]
        public int PackageId { get; set; }
        
        [StringLength(50)]
        public string? PaymentMethod { get; set; }
    }
} 