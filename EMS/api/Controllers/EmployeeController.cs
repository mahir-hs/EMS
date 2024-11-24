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
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController(IEmployeeService context) : ControllerBase
    {
        private readonly IEmployeeService _context = context;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var fetch =  await _context.GetAllAsync();
            return Ok(fetch);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(int id)
        {
            var fetch = await _context.GetByIdAsync(id);
            return Ok(fetch);
        }

        [HttpPost]
        [Route("add")]
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
        [Route("update")]
        public async Task<IActionResult> Update(int id,[FromBody] EmployeeUpdateDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.UpdateAsync(id,dto));
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            return Ok(await _context.DeleteAsync(Id));
        }


    }
}