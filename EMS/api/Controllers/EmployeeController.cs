using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Employees;
using api.Services;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService context) : ControllerBase
    {
        private readonly IEmployeeService _context = context;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.GetAllAsync());
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            return Ok(await _context.GetByIdAsync(id));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.AddAsync(dto));
        }

        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] EmployeeUpdateDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.UpdateAsync(id,dto));
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] int Id)
        {
            return Ok(await _context.DeleteAsync(Id));
        }


    }
}