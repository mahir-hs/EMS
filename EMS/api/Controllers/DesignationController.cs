using api.Dto.Designation;
using api.Dto.Employees;
using api.Models;
using api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/designation")]
    [ApiController]
    [Authorize]
    public class DesignationController(IDesignationService context, ILogger<DesignationController> logger) :ControllerBase
    {
        private readonly IDesignationService _context = context;
        private readonly ILogger<DesignationController> _logger = logger;



        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _context.GetAllAsync();

                if (!result.Success)
                {
                    _logger.LogWarning("No Designation found.");
                    return NotFound(result);
                }

                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Designation Controller");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                var result = await _context.GetByIdAsync(id);

                if (!result.Success)
                {
                    _logger.LogWarning("No Designation found.");
                    return NotFound(new { message = "No Designation found." });
                }

                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Designation Controller Get()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] DesignationCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to add new Designation");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to add new Designation", "400"));
            }
            try
            {
                var result = await _context.AddAsync(dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to add new Designation");
                    return BadRequest(result);
                }
                return StatusCode(201, new { message = "Designation added successfully.", data = result.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the Designation.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] DesignationUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to update the Designation");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to update the Designation", "400"));
            }
            try
            {
                var result = await _context.UpdateAsync(id, dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update the Department");
                    return StatusCode(500, result);
                } 
                return Ok(new { message = "Designation updated successfully.", data = result.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the designation.");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int Id)
        {
            try
            {
                var result = await _context.DeleteAsync(Id);
                if (!result.Success)
                {
                    return StatusCode(500, result);
                }
                return Ok(new { message = "Designation deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the Designation.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }

        }
    }
}
