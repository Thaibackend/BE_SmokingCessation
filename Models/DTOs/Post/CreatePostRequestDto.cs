namespace SmokingQuitSupportAPI.Models.DTOs.Post
{
    public class CreatePostRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
} 