using api.Dto.Department;
using api.Dto.Designation;
using api.Models;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(IDepartmentService context,ILogger<DepartmentController> logger) : ControllerBase
    {
        private readonly IDepartmentService _context = context;
        private readonly ILogger<DepartmentController> _logger = logger;
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _context.GetAllAsync();

                if (!result.Success)
                {
                    _logger.LogWarning("No departments found.");
                    return NotFound(result);
                }

                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in departments Controller");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
            
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                var department = await _context.GetByIdAsync(id);

                if (!department.Success)
                {
                    _logger.LogWarning("No department found.");
                    return NotFound(department);
                }

                return Ok(department.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in department Controller Get()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] DepartmentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addedDepartment = await _context.AddAsync(dto);
                if (!addedDepartment.Success)
                {
                    _logger.LogWarning("Failed to add new Department");
                    return BadRequest(addedDepartment);
                }
                return CreatedAtAction("GetById", new { id = addedDepartment.Result!.Id }, addedDepartment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the Department.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] DepartmentUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _context.UpdateAsync(id, dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update the Department");
                    return StatusCode(500,result);
                }
                return Ok(new { message = "Department updated successfully.", data = result.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Department.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int Id)
        {
            try
            {
                var result = await _context.DeleteAsync(Id);
                if(!result.Success)
                {
                    return StatusCode(500, result);
                }
                return Ok(new { message = "Department deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the Department.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

    }
}
