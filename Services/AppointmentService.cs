using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Appointment;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentRequestDto request, int memberId)
        {
            var appointment = new Appointment
            {
                MemberId = memberId,
                CoachId = request.CoachId,
                AppointmentDate = request.AppointmentDate,
                Notes = request.Notes,
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow
            };

            var createdAppointment = await _appointmentRepository.AddAsync(appointment);
            return _mapper.Map<AppointmentDto>(createdAppointment);
        }

        public async Task<IEnumerable<AppointmentDto>> GetUserAppointmentsAsync(int userId)
        {
            var appointments = await _appointmentRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetCoachAppointmentsAsync(int coachId)
        {
            var appointments = await _appointmentRepository.GetByCoachIdAsync(coachId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto?> UpdateAppointmentStatusAsync(int id, string status)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return null;

            appointment.Status = status;
            var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);
            return _mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return false;

            await _appointmentRepository.DeleteAsync(id);
            return true;
        }
    }
} 