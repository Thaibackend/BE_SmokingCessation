using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Activity;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class ActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository activityRepository, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<ActivityDto> CreateActivityAsync(int userId, CreateActivityRequestDto request)
        {
            var activity = new Activity
            {
                UserId = userId,
                Type = request.Type,
                Description = request.Description,
                Date = request.Date,
                CigarettesSmoked = request.CigarettesSmoked,
                MoneySaved = request.MoneySaved,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _activityRepository.AddAsync(activity);
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetUserActivitiesAsync(int userId)
        {
            var activities = await _activityRepository.GetUserActivitiesAsync(userId);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<ActivityDto?> GetActivityAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            return activity != null ? _mapper.Map<ActivityDto>(activity) : null;
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var activities = await _activityRepository.GetActivitiesByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task DeleteActivityAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity != null)
            {
                await _activityRepository.DeleteAsync(activity);
            }
        }
    }
} 