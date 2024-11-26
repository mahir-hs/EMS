using api.Dto.Department;
using api.Dto.Designation;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController(IDepartmentService context) : ControllerBase
    {
        private readonly IDepartmentService _context = context;

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
        public async Task<IActionResult> Add([FromBody] DepartmentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _context.AddAsync(dto));
        }

        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromQuery] int id,[FromBody] DepartmentUpdateDto dto)
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
