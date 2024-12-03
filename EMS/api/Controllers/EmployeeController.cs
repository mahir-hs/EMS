using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Employees;
using api.Models;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService context, ILogger<EmployeeController> logger) : ControllerBase
    {
        private readonly IEmployeeService _context = context;
        private readonly ILogger<EmployeeController> _logger = logger;

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
                _logger.LogError(ex, "An unexpected error occurred in Employee Controller GetAll()");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

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

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addedEmployee = await _context.AddAsync(dto);
                if (!addedEmployee.Success)
                {
                    _logger.LogWarning("Failed to add new employee");
                    return BadRequest(addedEmployee);
                }
                return CreatedAtAction("GetById", new { id = addedEmployee.Result!.Id }, addedEmployee.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the employee.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] EmployeeUpdateDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _context.UpdateAsync(id, dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update the Department");
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
                return Ok(new { message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employee.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


    }
}