namespace SmokingQuitSupportAPI.Models.DTOs.CoachApplication
{
    public class CreateCoachApplicationRequestDto
    {
        public string Qualifications { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string Motivation { get; set; } = string.Empty;
    }
} 