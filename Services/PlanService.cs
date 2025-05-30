using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Plan;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class PlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly IMapper _mapper;

        public PlanService(IPlanRepository planRepository, IMapper mapper)
        {
            _planRepository = planRepository;
            _mapper = mapper;
        }

        public async Task<PlanDto> CreatePlanAsync(CreatePlanRequestDto request)
        {
            var plan = new Plan
            {
                Name = request.Name,
                Description = request.Description,
                PackageId = request.PackageId,
                MemberId = request.MemberId,
                CoachId = request.CoachId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _planRepository.AddAsync(plan);
            
            // Get plan with details for mapping
            var plans = await _planRepository.GetUserPlansAsync(request.MemberId);
            var createdPlan = plans.FirstOrDefault(p => p.PlanId == plan.PlanId);
            
            return _mapper.Map<PlanDto>(createdPlan);
        }

        public async Task<IEnumerable<PlanDto>> GetUserPlansAsync(int userId)
        {
            var plans = await _planRepository.GetUserPlansAsync(userId);
            return _mapper.Map<IEnumerable<PlanDto>>(plans);
        }

        public async Task<IEnumerable<PlanDto>> GetCoachPlansAsync(int coachId)
        {
            var plans = await _planRepository.GetCoachPlansAsync(coachId);
            return _mapper.Map<IEnumerable<PlanDto>>(plans);
        }

        public async Task<PlanDto?> GetPlanAsync(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);
            return plan != null ? _mapper.Map<PlanDto>(plan) : null;
        }

        public async Task UpdatePlanStatusAsync(int id, string status)
        {
            var plan = await _planRepository.GetByIdAsync(id);
            if (plan != null)
            {
                plan.Status = status;
                plan.UpdatedAt = DateTime.UtcNow;
                await _planRepository.UpdateAsync(plan);
            }
        }

        public async Task DeletePlanAsync(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);
            if (plan != null)
            {
                await _planRepository.DeleteAsync(plan);
            }
        }
    }
} 