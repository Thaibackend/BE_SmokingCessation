namespace SmokingQuitSupportAPI.Models.DTOs.Achievement
{
    public class AchievementDto
    {
        public int AchievementId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Criteria { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 