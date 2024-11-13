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
            var fetch = await _context.GetAllLogsAsync();
            return Ok(fetch);
        }

        [HttpGet]
        [Route("get-employee")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var fetch = await _context.GetLogsForEmployeeAsync(id);
            return Ok(fetch);
        }
    }
}
