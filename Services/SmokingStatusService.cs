using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.SmokingStatus;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class SmokingStatusService
    {
        private readonly ISmokingStatusRepository _smokingStatusRepository;
        private readonly IMapper _mapper;

        public SmokingStatusService(ISmokingStatusRepository smokingStatusRepository, IMapper mapper)
        {
            _smokingStatusRepository = smokingStatusRepository;
            _mapper = mapper;
        }

        public async Task<SmokingStatusDto> CreateOrUpdateStatusAsync(int userId, CreateSmokingStatusRequestDto request)
        {
            var smokingStatus = new SmokingStatus
            {
                UserId = userId,
                QuitDate = request.QuitDate,
                CigarettesPerDay = request.CigarettesPerDay,
                CostPerPack = request.CostPerPack,
                CigarettesPerPack = request.CigarettesPerPack,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Calculate statistics
            var daysSinceQuit = (DateTime.UtcNow - request.QuitDate).Days;
            smokingStatus.DaysSmokeFree = Math.Max(0, daysSinceQuit);
            smokingStatus.CigarettesAvoided = smokingStatus.DaysSmokeFree * request.CigarettesPerDay;
            smokingStatus.MoneySaved = (smokingStatus.CigarettesAvoided / (decimal)request.CigarettesPerPack) * request.CostPerPack;

            await _smokingStatusRepository.AddAsync(smokingStatus);
            return _mapper.Map<SmokingStatusDto>(smokingStatus);
        }

        public async Task<SmokingStatusDto?> GetUserCurrentStatusAsync(int userId)
        {
            var status = await _smokingStatusRepository.GetUserCurrentStatusAsync(userId);
            if (status == null) return null;

            // Update real-time statistics
            var daysSinceQuit = (DateTime.UtcNow - status.QuitDate).Days;
            status.DaysSmokeFree = Math.Max(0, daysSinceQuit);
            status.CigarettesAvoided = status.DaysSmokeFree * status.CigarettesPerDay;
            status.MoneySaved = (status.CigarettesAvoided / (decimal)status.CigarettesPerPack) * status.CostPerPack;

            return _mapper.Map<SmokingStatusDto>(status);
        }

        public async Task<IEnumerable<SmokingStatusDto>> GetUserStatusHistoryAsync(int userId)
        {
            var statuses = await _smokingStatusRepository.GetUserStatusHistoryAsync(userId);
            return _mapper.Map<IEnumerable<SmokingStatusDto>>(statuses);
        }
    }
} 