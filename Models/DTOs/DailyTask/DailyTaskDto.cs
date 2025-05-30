namespace SmokingQuitSupportAPI.Models.DTOs.DailyTask
{
    public class DailyTaskDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int? PlanId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
} 