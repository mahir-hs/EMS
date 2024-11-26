using api.Dto.Designation;
using api.Dto.Employees;
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
        public async Task<IActionResult> Add([FromBody] DesignationCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.AddAsync(dto));
        }

        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] DesignationUpdateDto dto)
        {
            if (!ModelState.IsValid)
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
