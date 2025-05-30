namespace SmokingQuitSupportAPI.Models.DTOs.Activity
{
    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int CigarettesSmoked { get; set; }
        public decimal MoneySaved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 