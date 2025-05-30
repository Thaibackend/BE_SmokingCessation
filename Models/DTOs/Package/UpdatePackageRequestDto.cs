using System.ComponentModel.DataAnnotations;

namespace SmokingQuitSupportAPI.Models.DTOs.Package
{
    public class UpdatePackageRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Range(1, int.MaxValue)]
        public int DurationDays { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
} 