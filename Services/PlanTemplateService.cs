using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.PlanTemplate;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class PlanTemplateService
    {
        private readonly IPlanTemplateRepository _planTemplateRepository;
        private readonly IMapper _mapper;

        public PlanTemplateService(IPlanTemplateRepository planTemplateRepository, IMapper mapper)
        {
            _planTemplateRepository = planTemplateRepository;
            _mapper = mapper;
        }

        public async Task<PlanTemplateDto> CreateTemplateAsync(CreatePlanTemplateRequestDto request)
        {
            var template = new PlanTemplate
            {
                Name = request.Name,
                Description = request.Description,
                DurationDays = request.DurationDays,
                Difficulty = request.Difficulty,
                Category = request.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _planTemplateRepository.AddAsync(template);
            return _mapper.Map<PlanTemplateDto>(template);
        }

        public async Task<IEnumerable<PlanTemplateDto>> GetActiveTemplatesAsync()
        {
            var templates = await _planTemplateRepository.GetActiveTemplatesAsync();
            return _mapper.Map<IEnumerable<PlanTemplateDto>>(templates);
        }

        public async Task<IEnumerable<PlanTemplateDto>> GetTemplatesByCategoryAsync(string category)
        {
            var templates = await _planTemplateRepository.GetTemplatesByCategoryAsync(category);
            return _mapper.Map<IEnumerable<PlanTemplateDto>>(templates);
        }

        public async Task<PlanTemplateDto?> GetTemplateByIdAsync(int templateId)
        {
            var template = await _planTemplateRepository.GetByIdAsync(templateId);
            return template != null ? _mapper.Map<PlanTemplateDto>(template) : null;
        }

        public async Task<PlanTemplateDto?> UpdateTemplateAsync(int templateId, CreatePlanTemplateRequestDto request)
        {
            var template = await _planTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
                return null;

            template.Name = request.Name;
            template.Description = request.Description;
            template.DurationDays = request.DurationDays;
            template.Difficulty = request.Difficulty;
            template.Category = request.Category;
            template.UpdatedAt = DateTime.UtcNow;

            await _planTemplateRepository.UpdateAsync(template);
            return _mapper.Map<PlanTemplateDto>(template);
        }
    }
} 