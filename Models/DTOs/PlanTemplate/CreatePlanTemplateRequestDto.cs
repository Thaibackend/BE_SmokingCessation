namespace SmokingQuitSupportAPI.Models.DTOs.PlanTemplate
{
    public class CreatePlanTemplateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
} 