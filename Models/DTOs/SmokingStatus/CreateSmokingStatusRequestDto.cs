namespace SmokingQuitSupportAPI.Models.DTOs.SmokingStatus
{
    public class CreateSmokingStatusRequestDto
    {
        public DateTime QuitDate { get; set; }
        public int CigarettesPerDay { get; set; }
        public decimal CostPerPack { get; set; }
        public int CigarettesPerPack { get; set; }
    }
} 