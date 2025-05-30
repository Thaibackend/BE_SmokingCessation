namespace SmokingQuitSupportAPI.Models.DTOs.SmokingStatus
{
    public class SmokingStatusDto
    {
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public DateTime QuitDate { get; set; }
        public int CigarettesPerDay { get; set; }
        public decimal CostPerPack { get; set; }
        public int CigarettesPerPack { get; set; }
        public string Status { get; set; } = string.Empty;
        public int DaysSmokeFree { get; set; }
        public decimal MoneySaved { get; set; }
        public int CigarettesAvoided { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 