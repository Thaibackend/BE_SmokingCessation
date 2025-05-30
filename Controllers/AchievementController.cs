using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmokingQuitSupportAPI.Models.DTOs.Achievement;
using SmokingQuitSupportAPI.Services;
using System.Security.Claims;

namespace SmokingQuitSupportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AchievementController : ControllerBase
    {
        private readonly AchievementService _achievementService;

        public AchievementController(AchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AchievementDto>> CreateAchievement([FromBody] CreateAchievementRequestDto request)
        {
            try
            {
                var achievement = await _achievementService.CreateAchievementAsync(request);
                return Ok(achievement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AchievementDto>>> GetActiveAchievements()
        {
            try
            {
                var achievements = await _achievementService.GetActiveAchievementsAsync();
                return Ok(achievements);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<UserAchievementDto>>> GetUserAchievements()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userAchievements = await _achievementService.GetUserAchievementsAsync(userId);
                return Ok(userAchievements);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("unlock/{achievementId}")]
        public async Task<ActionResult<UserAchievementDto>> UnlockAchievement(int achievementId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userAchievement = await _achievementService.UnlockAchievementAsync(userId, achievementId);
                
                if (userAchievement == null)
                    return BadRequest(new { message = "Achievement already unlocked or not found" });

                return Ok(userAchievement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 