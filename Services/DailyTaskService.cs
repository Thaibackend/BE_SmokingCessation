using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.DailyTask;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class DailyTaskService
    {
        private readonly IDailyTaskRepository _dailyTaskRepository;
        private readonly IMapper _mapper;

        public DailyTaskService(IDailyTaskRepository dailyTaskRepository, IMapper mapper)
        {
            _dailyTaskRepository = dailyTaskRepository;
            _mapper = mapper;
        }

        public async Task<DailyTaskDto> CreateTaskAsync(int userId, CreateDailyTaskRequestDto request)
        {
            var task = new DailyTask
            {
                UserId = userId,
                PlanId = request.PlanId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = "Pending",
                Priority = request.Priority,
                CreatedAt = DateTime.UtcNow
            };

            await _dailyTaskRepository.AddAsync(task);
            return _mapper.Map<DailyTaskDto>(task);
        }

        public async Task<IEnumerable<DailyTaskDto>> GetUserTasksAsync(int userId)
        {
            var tasks = await _dailyTaskRepository.GetUserTasksAsync(userId);
            return _mapper.Map<IEnumerable<DailyTaskDto>>(tasks);
        }

        public async Task<IEnumerable<DailyTaskDto>> GetTasksByDateAsync(int userId, DateTime date)
        {
            var tasks = await _dailyTaskRepository.GetTasksByDateAsync(userId, date);
            return _mapper.Map<IEnumerable<DailyTaskDto>>(tasks);
        }

        public async Task<IEnumerable<DailyTaskDto>> GetPendingTasksAsync(int userId)
        {
            var tasks = await _dailyTaskRepository.GetPendingTasksAsync(userId);
            return _mapper.Map<IEnumerable<DailyTaskDto>>(tasks);
        }

        public async Task<DailyTaskDto?> CompleteTaskAsync(int taskId, int userId)
        {
            var task = await _dailyTaskRepository.GetByIdAsync(taskId);
            if (task == null || task.UserId != userId)
                return null;

            task.Status = "Completed";
            task.CompletedAt = DateTime.UtcNow;
            await _dailyTaskRepository.UpdateAsync(task);

            return _mapper.Map<DailyTaskDto>(task);
        }

        public async Task<DailyTaskDto?> UpdateTaskStatusAsync(int taskId, int userId, string status)
        {
            var task = await _dailyTaskRepository.GetByIdAsync(taskId);
            if (task == null || task.UserId != userId)
                return null;

            task.Status = status;
            if (status == "Completed")
                task.CompletedAt = DateTime.UtcNow;

            await _dailyTaskRepository.UpdateAsync(task);
            return _mapper.Map<DailyTaskDto>(task);
        }
    }
} 