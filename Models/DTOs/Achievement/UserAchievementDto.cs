namespace SmokingQuitSupportAPI.Models.DTOs.Achievement
{
    public class UserAchievementDto
    {
        public int UserAchievementId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int AchievementId { get; set; }
        public string AchievementName { get; set; } = string.Empty;
        public string AchievementIcon { get; set; } = string.Empty;
        public DateTime UnlockedAt { get; set; }
    }
} 