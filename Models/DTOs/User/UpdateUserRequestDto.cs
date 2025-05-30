using System.ComponentModel.DataAnnotations;

namespace SmokingQuitSupportAPI.Models.DTOs.User
{
    public class UpdateUserRequestDto
    {
        [StringLength(50)]
        public string? Username { get; set; }
        
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? FullName { get; set; }
    }
} 