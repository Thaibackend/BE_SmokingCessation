namespace SmokingQuitSupportAPI.Models.DTOs.Plan
{
    public class CreatePlanRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PackageId { get; set; }
        public int MemberId { get; set; }
        public int? CoachId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
} 