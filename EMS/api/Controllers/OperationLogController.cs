using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/log")]
    [ApiController]

    public class OperationLogController(IOperationLogService context, ILogger<OperationLogController> logger) : ControllerBase
    {
        private readonly IOperationLogService _context = context;
        private readonly ILogger<OperationLogController> _logger = logger;

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _context.GetAllLogsAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No Logs found.");
                    return NotFound(new { message = "No Logs found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Log Controller GetAll()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
            
        }

        [HttpGet("get-employee")]
        public async Task<IActionResult> GetEmployee([FromQuery] int id)
        {
            try
            {
                var result = await _context.GetLogsForEmployeeAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("No Logs found.");
                    return NotFound(new { message = "No Logs found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Logs Controller Get()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("get-attendance")]
        public async Task<IActionResult> GetAttendance([FromQuery] int id)
        {
            try
            {
                var result = await _context.GetLogsForAttendanceAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("No Logs found.");
                    return NotFound(new { message = "No Logs found." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Logs Controller Get()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


    }
}
