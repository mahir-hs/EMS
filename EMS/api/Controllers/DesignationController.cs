using api.Dtos.Designation;
using api.Dtos.Employees;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/designation")]
    [ApiController]
    public class DesignationController(IDesignationService context):ControllerBase
    {
        private readonly IDesignationService _context = context;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var fetch = await _context.GetAllAsync();
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
        public async Task<IActionResult> Add([FromBody] DesignationCreateDto dto)
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
        public async Task<IActionResult> Update([FromBody] DesignationUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.UpdateAsync(dto));
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            return Ok(await _context.DeleteAsync(Id));
        }
    }
}
