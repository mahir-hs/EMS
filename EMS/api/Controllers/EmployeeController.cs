using api.Dto.Employees;
using api.Models;
using api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmployeeController(IEmployeeService context, ILogger<EmployeeController> logger) : ControllerBase
    {
        private readonly IEmployeeService _context = context;
        private readonly ILogger<EmployeeController> _logger = logger;
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _context.GetAllAsync();

                if (!employees.Success)
                {
                    _logger.LogWarning("No employees found.");
                    return NotFound(employees);
                }

                return Ok(employees.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Employee Controller");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                var employee = await _context.GetByIdAsync(id);

                if (!employee.Success)
                {
                    _logger.LogWarning("No employee found.");
                    return NotFound(employee);
                }

                return Ok(employee.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in Employee Controller Get()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to add new Employee");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to add new Employee", "400"));
            }
            try
            {
                Console.WriteLine(dto);
                var addedEmployee = await _context.AddAsync(dto);
                if (!addedEmployee.Success)
                {
                    _logger.LogWarning("Failed to add new employee");
                    return BadRequest(addedEmployee);
                }
                return StatusCode(201, new { message = "Employee added successfully.", data = addedEmployee.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the employee.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] EmployeeUpdateDto dto)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to update Employee");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to update Employee", "400"));
            }
            try
            {
                var result = await _context.UpdateAsync(id, dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update the employee");
                    return StatusCode(500, result);
                }
                return Ok(new { message = "employee updated successfully.", data = result.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the employee.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                var result = await _context.DeleteAsync(id);
                if (!result.Success)
                {
                    return StatusCode(500, result);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employee.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


    }
}