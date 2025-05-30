namespace SmokingQuitSupportAPI.Models.DTOs.Achievement
{
    public class CreateAchievementRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Criteria { get; set; } = string.Empty;
        public int Points { get; set; }
    }
} 