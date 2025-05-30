using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmokingQuitSupportAPI.Models.DTOs.CoachApplication;
using SmokingQuitSupportAPI.Services;
using System.Security.Claims;

namespace SmokingQuitSupportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoachApplicationController : ControllerBase
    {
        private readonly CoachApplicationService _coachApplicationService;

        public CoachApplicationController(CoachApplicationService coachApplicationService)
        {
            _coachApplicationService = coachApplicationService;
        }

        [HttpPost]
        public async Task<ActionResult<CoachApplicationDto>> CreateApplication([FromBody] CreateCoachApplicationRequestDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var application = await _coachApplicationService.CreateApplicationAsync(userId, request);
                return Ok(application);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CoachApplicationDto>>> GetPendingApplications()
        {
            try
            {
                var applications = await _coachApplicationService.GetPendingApplicationsAsync();
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-application")]
        public async Task<ActionResult<CoachApplicationDto>> GetMyApplication()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var application = await _coachApplicationService.GetUserApplicationAsync(userId);
                
                if (application == null)
                    return NotFound(new { message = "No application found" });

                return Ok(application);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/review")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CoachApplicationDto>> ReviewApplication(int id, [FromBody] ReviewApplicationRequest request)
        {
            try
            {
                var application = await _coachApplicationService.ReviewApplicationAsync(id, request.Status, request.ReviewNotes);
                
                if (application == null)
                    return NotFound(new { message = "Application not found" });

                return Ok(application);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class ReviewApplicationRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? ReviewNotes { get; set; }
    }
} 