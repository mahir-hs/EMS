using api.Controllers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using ClosedXML.Excel;
using CsvHelper;
using DocumentFormat.OpenXml.Bibliography;
using System.Globalization;
using System.IO;

namespace api.Services
{
    public class DownloadService(IEmployeeRepository employee,
                                    IDepartmentRepository department,
                                    IDesignationRepository designation,
                                    IOperationLogRepository operationLog,
                                    IEmployeeAttendanceRepository employeeAttendance,
                                    ILogger<DownloadService> logger) : IDownloadService
    {

        private readonly IEmployeeRepository _employeeRepository = employee;
        private readonly IDepartmentRepository _departmentRepository = department;
        private readonly IDesignationRepository _designationRepository = designation;
        private readonly IOperationLogRepository _operationLogRepository = operationLog;
        private readonly IEmployeeAttendanceRepository _employeeAttendanceRepository = employeeAttendance;
        private readonly ILogger<DownloadService> _logger = logger;
        

        public async Task<ApiResponse> GetEmployeeAttendanceCSVFile(int id)
        {
            try
            {
                var response = await _employeeAttendanceRepository.GetUserAttendance(id);
                if (!response.Success)
                {
                    return new ApiResponse(null, false, response.Message, response.Type);
                }

                var employeeAttendances = response.Result as IEnumerable<EmployeeAttendance>;
                using var memoryStream = new MemoryStream();
                using var writer = new StreamWriter(memoryStream);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.WriteField("ID");
                csv.WriteField("In Time");
                csv.WriteField("Out Time");
                await csv.NextRecordAsync();

                foreach (var employeeAttendance in employeeAttendances)
                {
                    csv.WriteField(employeeAttendance.Id);
                    csv.WriteField(employeeAttendance.CheckInTime);
                    csv.WriteField(employeeAttendance.CheckOutTime);
                    await csv.NextRecordAsync();
                }
                await writer.FlushAsync();
                memoryStream.Seek(0, SeekOrigin.Begin);
                _logger.LogInformation("CSV File Size: {Size} bytes", memoryStream.Length);

                return new ApiResponse(memoryStream.ToArray(), true, "Success", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to CSV From service.");
                return new ApiResponse(null, false, "Failed to export to CSV From service.", "500");
            }
        }

        public async Task<ApiResponse> GetEmployeeAttendanceExcelFile(int id)
        {

            try
            {
                var response = await _employeeAttendanceRepository.GetUserAttendance(id);
                if (!response.Success)
                {
                    return new ApiResponse(null, false, response.Message, response.Type);
                }

                var employeeAttendances = response.Result as IEnumerable<EmployeeAttendance>;

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employee Attendances of UserID" + id.ToString());

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "In Time";
                worksheet.Cell(1, 3).Value = "Out Time";

                var row = 2;
                foreach (var emp in employeeAttendances)
                {
                    worksheet.Cell(row, 1).Value = emp.Id;
                    worksheet.Cell(row, 2).Value = emp.CheckInTime;
                    worksheet.Cell(row, 3).Value = emp.CheckOutTime;
                    row++;
                }
                using var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                return new ApiResponse(memoryStream.ToArray(), true, "Excel file created successfully", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to Excel From service.");
                return new ApiResponse(null, false, "Failed to export to Excel From service.", "500");
            }
        }

        public async Task<ApiResponse> GetEmployeeCSVFile()
        {
            try
            {
                
                var response = await _employeeRepository.GetAllAsync();
                if(!response.Success)
                {
                    return new ApiResponse(null, false, response.Message, response.Type);
                }
                var deptResponse = await _departmentRepository.GetAllAsync();
                if(!deptResponse.Success)
                {
                    return new ApiResponse(null, false, deptResponse.Message, deptResponse.Type);
                }

                var designationResponse = await _designationRepository.GetAllAsync();
                if(!designationResponse.Success)
                {
                    return new ApiResponse(null, false, designationResponse.Message, designationResponse.Type);
                }

                var employees = response.Result as IEnumerable<Employee>;
                var department = deptResponse.Result as IEnumerable<Models.Department>;
                var designation = designationResponse.Result as IEnumerable<Designation>;


                using var memoryStream = new MemoryStream();
                using var writer = new StreamWriter(memoryStream);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                
                csv.WriteField("ID");
                csv.WriteField("Name");
                csv.WriteField("Email");
                csv.WriteField("Department");
                csv.WriteField("Designation");
                csv.WriteField("Phone");
                csv.WriteField("Address");
                csv.WriteField("DateOfBirth");
                await csv.NextRecordAsync();

                foreach (var emp in employees)
                {
                    csv.WriteField(emp.Id);
                    csv.WriteField($"{emp.FirstName} {emp.LastName}");
                    csv.WriteField(emp.Email);
                    csv.WriteField(department?.FirstOrDefault(x => x.Id == emp.DepartmentId)?.Dept);
                    csv.WriteField(designation?.FirstOrDefault(x => x.Id == emp.DesignationId)?.Role);
                    csv.WriteField(emp.Phone);
                    csv.WriteField(emp.Address);
                    csv.WriteField(emp.DateOfBirth.ToString("yyyy-MM-dd"));
                    await csv.NextRecordAsync();
                }

                await writer.FlushAsync();
                memoryStream.Seek(0, SeekOrigin.Begin);
                _logger.LogInformation("CSV File Size: {Size} bytes", memoryStream.Length);

                return new ApiResponse(memoryStream.ToArray(), true, "Success", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to CSV From service.");
                return new ApiResponse(null, false, "Failed to export to CSV From service.", "500");
            }
        }

        public async Task<ApiResponse> GetEmployeeExcelFile()
        {
            try
            {
                var response = await _employeeRepository.GetAllAsync();
                if (!response.Success)
                {
                    return new ApiResponse(null, false, response.Message, response.Type);
                }

                var deptResponse = await _departmentRepository.GetAllAsync();
                if (!deptResponse.Success)
                {
                    return new ApiResponse(null, false, deptResponse.Message, deptResponse.Type);
                }
                var departments = deptResponse.Result as IEnumerable<Models.Department>;

                var designationResponse = await _designationRepository.GetAllAsync();
                if (!designationResponse.Success)
                {
                    return new ApiResponse(null, false, designationResponse.Message, designationResponse.Type);
                }
                var designations = designationResponse.Result as IEnumerable<Designation>;

                var employees = response.Result as IEnumerable<Employee>;

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employees");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Department";
                worksheet.Cell(1, 5).Value = "Designation";
                worksheet.Cell(1, 6).Value = "Phone";
                worksheet.Cell(1, 7).Value = "Address";
                worksheet.Cell(1, 8).Value = "DateOfBirth";

                var row = 2; 
                foreach (var emp in employees)
                {
                    worksheet.Cell(row, 1).Value = emp.Id;
                    worksheet.Cell(row, 2).Value = $"{emp.FirstName} {emp.LastName}";
                    worksheet.Cell(row, 3).Value = emp.Email;
                    worksheet.Cell(row, 4).Value = departments?.FirstOrDefault(x => x.Id == emp.DepartmentId)?.Dept;
                    worksheet.Cell(row, 5).Value = designations?.FirstOrDefault(x => x.Id == emp.DesignationId)?.Role;
                    worksheet.Cell(row, 6).Value = emp.Phone;
                    worksheet.Cell(row, 7).Value = emp.Address;
                    worksheet.Cell(row, 8).Value = emp.DateOfBirth.ToString("yyyy-MM-dd");

                    row++;
                }

               
                using var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                return new ApiResponse(memoryStream.ToArray(), true, "Excel file created successfully", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting employee data to Excel.");
                return new ApiResponse(null, false, "Failed to export to Excel", "500");
            }
        }
    }
}
