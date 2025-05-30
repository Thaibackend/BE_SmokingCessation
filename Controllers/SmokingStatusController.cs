using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmokingQuitSupportAPI.Models.DTOs.SmokingStatus;
using SmokingQuitSupportAPI.Services;
using System.Security.Claims;

namespace SmokingQuitSupportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SmokingStatusController : ControllerBase
    {
        private readonly SmokingStatusService _smokingStatusService;

        public SmokingStatusController(SmokingStatusService smokingStatusService)
        {
            _smokingStatusService = smokingStatusService;
        }

        [HttpPost]
        public async Task<ActionResult<SmokingStatusDto>> CreateOrUpdateStatus([FromBody] CreateSmokingStatusRequestDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var status = await _smokingStatusService.CreateOrUpdateStatusAsync(userId, request);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("current")]
        public async Task<ActionResult<SmokingStatusDto>> GetCurrentStatus()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var status = await _smokingStatusService.GetUserCurrentStatusAsync(userId);
                
                if (status == null)
                    return NotFound(new { message = "No smoking status found" });

                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<SmokingStatusDto>>> GetStatusHistory()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var statuses = await _smokingStatusService.GetUserStatusHistoryAsync(userId);
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 