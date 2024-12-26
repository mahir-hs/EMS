using api.Models;

namespace api.Services.IServices
{
    public interface IDownloadService
    {
        Task<ApiResponse> GetEmployeeAttendanceCSVFile(int id);
        Task<ApiResponse> GetEmployeeAttendanceExcelFile(int id);
        Task<ApiResponse> GetEmployeeCSVFile();
        Task<ApiResponse> GetEmployeeExcelFile();
    }
}
