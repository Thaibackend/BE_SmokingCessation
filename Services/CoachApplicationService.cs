using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.CoachApplication;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class CoachApplicationService
    {
        private readonly ICoachApplicationRepository _coachApplicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CoachApplicationService(
            ICoachApplicationRepository coachApplicationRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _coachApplicationRepository = coachApplicationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CoachApplicationDto> CreateApplicationAsync(int userId, CreateCoachApplicationRequestDto request)
        {
            // Check if user already has a pending application
            var existingApplication = await _coachApplicationRepository.GetUserApplicationAsync(userId);
            if (existingApplication != null && existingApplication.Status == "Pending")
            {
                throw new InvalidOperationException("You already have a pending application.");
            }

            var application = new CoachApplication
            {
                UserId = userId,
                Qualifications = request.Qualifications,
                Experience = request.Experience,
                Motivation = request.Motivation,
                Status = "Pending",
                ApplicationDate = DateTime.UtcNow
            };

            await _coachApplicationRepository.AddAsync(application);
            
            // Get with user details for mapping
            var applications = await _coachApplicationRepository.GetPendingApplicationsAsync();
            var created = applications.FirstOrDefault(a => a.ApplicationId == application.ApplicationId);
            
            return _mapper.Map<CoachApplicationDto>(created);
        }

        public async Task<IEnumerable<CoachApplicationDto>> GetPendingApplicationsAsync()
        {
            var applications = await _coachApplicationRepository.GetPendingApplicationsAsync();
            return _mapper.Map<IEnumerable<CoachApplicationDto>>(applications);
        }

        public async Task<CoachApplicationDto?> GetUserApplicationAsync(int userId)
        {
            var application = await _coachApplicationRepository.GetUserApplicationAsync(userId);
            return application != null ? _mapper.Map<CoachApplicationDto>(application) : null;
        }

        public async Task<CoachApplicationDto?> ReviewApplicationAsync(int applicationId, string status, string? reviewNotes = null)
        {
            var application = await _coachApplicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                return null;

            application.Status = status;
            application.ReviewedDate = DateTime.UtcNow;
            application.ReviewNotes = reviewNotes;

            // If approved, update user role to Coach
            if (status == "Approved")
            {
                var user = await _userRepository.GetByIdAsync(application.UserId);
                if (user != null)
                {
                    user.Role = "Coach";
                    await _userRepository.UpdateAsync(user);
                }
            }

            await _coachApplicationRepository.UpdateAsync(application);
            
            // Get with user details for mapping
            var updatedApplication = await _coachApplicationRepository.GetUserApplicationAsync(application.UserId);
            return _mapper.Map<CoachApplicationDto>(updatedApplication);
        }
    }
} 