using api.Repository;
using api.Repository.IRepository;
using api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DownloadController(IDownloadService context,ILogger<DownloadController> logger) : ControllerBase
    {
        private readonly IDownloadService _context = context;
        private readonly ILogger _logger = logger;

        [HttpGet("export-to-csv")]
        [Produces("text/csv")]
        public async Task<IActionResult> ExportToCSV()
        {
            try
            {
                var apiResponse = await _context.GetEmployeeCSVFile(); 
                if (!apiResponse.Success)
                {
                    return StatusCode(500, apiResponse.Message); 
                }

                var fileContent = apiResponse.Result as byte[];
                if(fileContent == null)
                {
                    return BadRequest(
                        new
                        {
                            message = "Failed to export employee data to CSV."
                        });
                }
                return new FileContentResult(fileContent, "text/csv")
                {
                    FileDownloadName = "EmployeeList.csv"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting employee data to CSV.");
                return StatusCode(500, "An error occurred while exporting data.");
            }
        }

        [HttpGet("export-to-excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var apiResponse = await _context.GetEmployeeExcelFile();
                if (!apiResponse.Success)
                {
                    return StatusCode(500, apiResponse.Message);
                }

                // Return the Excel file as a downloadable file
                var fileContent = apiResponse.Result as byte[];
                return new FileContentResult(fileContent!, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "EmployeeList.xlsx"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting employee data to Excel.");
                return StatusCode(500, "An error occurred while exporting data.");
            }
        }


    }
}
