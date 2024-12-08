using System.Data;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;
using Microsoft.Data.SqlClient;

namespace api.Repository
{
    public class EmployeeRepository(IFactoryDbContext context, IOperationLogService operationLogService,ILogger<EmployeeRepository> logger) : IEmployeeRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<EmployeeRepository> _logger = logger;

        public async Task<ApiResponse> AddAsync(Employee entity)
        {
            using var con = _context.SqlConnection();
            con.Open();

            try
            {
         
                var employee = await con.QueryFirstOrDefaultAsync<Employee>(
                    "dbo.AddEmployee",
                    new
                    {
                        entity.FirstName,
                        entity.LastName,
                        entity.Email,
                        entity.Phone,
                        entity.Address,
                        entity.DateOfBirth,
                        entity.DepartmentId,
                        entity.DesignationId
                    },
                    commandType: CommandType.StoredProcedure);

                if (employee == null)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Failed to add Employee.",
                        "500"
                    );
                }
                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Employee",
                        EntityId = employee.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateInsertDetails(employee)
                    };

                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee {EmployeeId}", employee.Id);
                }

                return new ApiResponse(
                employee,
                true,
                "Added Successfully",
                "200");
           
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while adding a employee: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while adding the employee.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a employee in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while adding the employee in repository.",
                "500");
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, Employee entity)
        {
            using var con = _context.SqlConnection();
            con.Open();

            try
            {
                var oldEntity = await GetByIdAsync(id);
                if (!oldEntity.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Employee not found for update in repository.",
                        "404"
                    );
                }
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                parameters.Add("FirstName", entity.FirstName);
                parameters.Add("LastName", entity.LastName);
                parameters.Add("Email", entity.Email);
                parameters.Add("Phone", entity.Phone);
                parameters.Add("Address", entity.Address);
                parameters.Add("DateOfBirth", entity.DateOfBirth);
                parameters.Add("DepartmentId", entity.DepartmentId);
                parameters.Add("DesignationId", entity.DesignationId);
                parameters.Add("ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


                var updatedEmployee = await con.QuerySingleOrDefaultAsync<Employee>(
                    "dbo.UpdateEmployee",
                  parameters,
                    commandType: CommandType.StoredProcedure);

                var errorMessage = parameters.Get<string>("ErrorMessage");
                var result = parameters.Get<int>("ReturnValue");

                if (result==-1)
                {
                    return new ApiResponse(
                        null,
                        false,
                        errorMessage,
                        "500"
                    );
                }

                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Employee",
                        EntityId = updatedEmployee!.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity.Result, updatedEmployee)
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee {EmployeeId}", id);
                }

                return new ApiResponse(
                updatedEmployee,
                true,
                "Updated Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while updating a employee: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while updating the employee.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating a employee in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while updating the employee in repository.",
                "500");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            con.Open();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                parameters.Add("ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await con.QuerySingleOrDefaultAsync<Employee>(
                    "dbo.DeleteEmployee",
                   parameters,
                    commandType: CommandType.StoredProcedure);


                var errorMessage = parameters.Get<string>("ErrorMessage");
                var result = parameters.Get<int>("ReturnValue");

                if (result == -1)
                {
                    return new ApiResponse(
                        null,
                        false,
                        errorMessage,
                        "500"
                    );

                }
                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Delete",
                        EntityName = "Employee",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = $"Deleted employee with id {id}"
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch(Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee {EmployeeId}", id);
                }

                return new ApiResponse(
                    null,
                    true,
                    "Deleted Successfully",
                    "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while deleting a employee: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while deleting the employee.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a employee in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while deleting the employee in repository.",
                "500");
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            con.Open();
            try
            {
                var result = await con.QueryAsync<Employee>("dbo.GetAllEmployees", commandType: CommandType.StoredProcedure);
                return new ApiResponse(result, true, "Fetched all Employees successfully!", "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching Employee in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while fetching Employee in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching Employee in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while fetching Employee in repository.",
                    "500"
                );
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            con.Open();
            try
            {
                var result = await con.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { id }, commandType: CommandType.StoredProcedure);
                return new ApiResponse(
                    result, true, "Fetched the employee successfully","200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching employee in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while fetching Employee in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching Employee in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while fetching Employee in repository.",
                    "500"
                );
            }

        }
    }
}
