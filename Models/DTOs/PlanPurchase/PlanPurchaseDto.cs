namespace SmokingQuitSupportAPI.Models.DTOs.PlanPurchase
{
    public class PlanPurchaseDto
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
    }
} 