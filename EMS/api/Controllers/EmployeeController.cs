using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Employees;
using api.Services;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController:ControllerBase
    {
        private readonly IEmployeeService _context;
        public EmployeeController(IEmployeeService context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fetch =  await _context.GetAllAsync();
            return Ok(fetch);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fetch = await _context.AddAsync(dto);
            return Ok(fetch);
        }
    }
}