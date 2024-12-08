using api.Dto.EmployeeAttendance;
using api.Dto.Employees;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/employee-attendance")]
    [Controller]
    public class EmployeeAttendanceController(IEmployeeAttendanceService context, ILogger<EmployeeAttendanceController> logger) :ControllerBase
    public class EmployeeAttendanceController(IEmployeeAttendanceService context, ILogger<EmployeeAttendanceController> logger) :ControllerBase
    {
        private readonly IEmployeeAttendanceService _context = context;
        private readonly ILogger<EmployeeAttendanceController> _logger = logger;
        

        [HttpGet("all")]
        private readonly ILogger<EmployeeAttendanceController> _logger = logger;
        

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _context.GetAllAttendanceAsync();

                if (!result.Success)
                {
                    _logger.LogWarning("No Attendance found.");
                    return NotFound(result);
                }

                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Attendance Controller");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
            
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserAttendance([FromQuery] int id)
        {
            try
            {
                var result = await _context.GetAllUserAttendance(id);

                if (!result.Success)
                {
                    _logger.LogWarning("No Attendance found.");
                    return NotFound(result);
                }

                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Attendance Controller");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
       
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] int id,[FromBody] EmployeeAttendanceCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _context.AddAttendanceAsync(id, dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to add new Attendance");
                    return BadRequest(result);
                }
                return CreatedAtAction("Add", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the Attendance.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
            
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] EmployeeAttendanceUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _context.UpdateAttendanceAsync(id, dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update the Attendance");
                    return StatusCode(500, result);
                }
               
                return Ok(new { message = "Attendance updated successfully.", data = result.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Attendance.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("get-single")]
        public async Task<IActionResult> GetSingleAttendance([FromQuery] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _context.GetAttendanceByAttendanceId(id);

                if (!result.Success)
                {
                    _logger.LogWarning("No Attendance found.");
                    return NotFound(result);
                }
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the Attendance.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


    }
}
