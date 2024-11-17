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
        public async Task<IActionResult> Add([FromBody] DepartmentCreateDto dto)
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
        public async Task<IActionResult> Update([FromBody] DepartmentUpdateDto dto)
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
