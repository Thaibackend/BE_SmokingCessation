namespace SmokingQuitSupportAPI.Models.DTOs.DailyTask
{
    public class CreateDailyTaskRequestDto
    {
        public int? PlanId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } = "Medium";
    }
} 