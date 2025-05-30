namespace SmokingQuitSupportAPI.Models.DTOs.Notification
{
    public class CreateNotificationRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
} 