using api.Models;

namespace api.Services.IServices
{
    public interface IDownloadService
    {
        Task<ApiResponse> GetEmployeeCSVFile();
        Task<ApiResponse> GetEmployeeExcelFile();
    }
}
