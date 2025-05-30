namespace SmokingQuitSupportAPI.Models.DTOs.CoachApplication
{
    public class CoachApplicationDto
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Qualifications { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string Motivation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? ReviewNotes { get; set; }
    }
} 