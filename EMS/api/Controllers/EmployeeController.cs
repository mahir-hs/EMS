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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var fetch = await _context.GetByIdAsync(id);
            return Ok(fetch);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fetch = await _context.AddAsync(dto);
            return Ok(fetch);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] EmployeeUpdateDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.UpdateAsync(dto));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            return Ok(await _context.DeleteAsync(Id));
        }


    }
}