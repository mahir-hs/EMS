using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/log")]
    [ApiController]

    public class OperationLogController(IOperationLogService context): ControllerBase
    {
        private readonly IOperationLogService _context = context;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.GetAllLogsAsync());
        }

        [HttpGet]
        [Route("get-employee")]
        public async Task<IActionResult> GetEmployee([FromQuery] int id)
        {
            return Ok(await _context.GetLogsForEmployeeAsync(id));
        }


    }
}
