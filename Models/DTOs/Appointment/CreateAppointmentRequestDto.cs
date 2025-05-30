namespace SmokingQuitSupportAPI.Models.DTOs.Appointment
{
    public class CreateAppointmentRequestDto
    {
        public int CoachId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
} 