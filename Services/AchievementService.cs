using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Achievement;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class AchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IMapper _mapper;

        public AchievementService(
            IAchievementRepository achievementRepository,
            IUserAchievementRepository userAchievementRepository,
            IMapper mapper)
        {
            _achievementRepository = achievementRepository;
            _userAchievementRepository = userAchievementRepository;
            _mapper = mapper;
        }

        public async Task<AchievementDto> CreateAchievementAsync(CreateAchievementRequestDto request)
        {
            var achievement = new Achievement
            {
                Name = request.Name,
                Description = request.Description,
                Icon = request.Icon,
                Criteria = request.Criteria,
                Points = request.Points,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _achievementRepository.AddAsync(achievement);
            return _mapper.Map<AchievementDto>(achievement);
        }

        public async Task<IEnumerable<AchievementDto>> GetActiveAchievementsAsync()
        {
            var achievements = await _achievementRepository.GetActiveAchievementsAsync();
            return _mapper.Map<IEnumerable<AchievementDto>>(achievements);
        }

        public async Task<IEnumerable<UserAchievementDto>> GetUserAchievementsAsync(int userId)
        {
            var userAchievements = await _userAchievementRepository.GetUserAchievementsAsync(userId);
            return _mapper.Map<IEnumerable<UserAchievementDto>>(userAchievements);
        }

        public async Task<UserAchievementDto?> UnlockAchievementAsync(int userId, int achievementId)
        {
            // Check if user already has this achievement
            var hasAchievement = await _userAchievementRepository.HasUserAchievementAsync(userId, achievementId);
            if (hasAchievement)
                return null;

            var userAchievement = new UserAchievement
            {
                UserId = userId,
                AchievementId = achievementId,
                UnlockedAt = DateTime.UtcNow
            };

            await _userAchievementRepository.AddAsync(userAchievement);
            
            // Get with details for mapping
            var userAchievements = await _userAchievementRepository.GetUserAchievementsAsync(userId);
            var created = userAchievements.FirstOrDefault(ua => ua.UserAchievementId == userAchievement.UserAchievementId);
            
            return _mapper.Map<UserAchievementDto>(created);
        }
    }
} 