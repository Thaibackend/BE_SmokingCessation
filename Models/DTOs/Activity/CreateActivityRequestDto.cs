namespace SmokingQuitSupportAPI.Models.DTOs.Activity
{
    public class CreateActivityRequestDto
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int CigarettesSmoked { get; set; }
        public decimal MoneySaved { get; set; }
    }
} 