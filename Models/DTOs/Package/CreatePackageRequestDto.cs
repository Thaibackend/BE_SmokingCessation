namespace SmokingQuitSupportAPI.Models.DTOs.Package
{
    public class CreatePackageRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
        public int DurationDays { get; set; }
    }
} 