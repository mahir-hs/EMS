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

        public async Task<Response<Employee?>> AddAsync(Employee entity)
        {
            using var con = _context.SqlConnection();
            con.Open();

            try
            {
         
                var employee = await con.QuerySingleOrDefaultAsync<Employee>(
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
                    return new Response<Employee?>(
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

                return new Response<Employee?>(
                employee,
                true,
                "Added Successfully",
                "200");
           
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while adding a employee: {Message}", sqlEx.Message);
                return new Response<Employee?>(
                null,
                false,
                "A database error occurred while adding the employee.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a employee in repository: {Message}", ex.Message);
                return new Response<Employee?>(
                null,
                false,
                "A database error occurred while adding the employee in repository.",
                "500");
            }
        }

        public async Task<Response<Employee?>> UpdateAsync(int id, Employee entity)
        {
            using var con = _context.SqlConnection();
            con.Open();

            try
            {
                var oldEntity = await GetByIdAsync(id);
                if (oldEntity == null) return null;

                var updatedEmployee = await con.QuerySingleOrDefaultAsync<Employee>(
                    "dbo.UpdateEmployee",
                    new
                    {
                        id,
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


                if (updatedEmployee == null)
                {
                    return new Response<Employee?>(
                        null,
                        false,
                        "Failed to update Employee.",
                        "500"
                    );
                }

                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Employee",
                        EntityId = updatedEmployee.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity.Result, updatedEmployee)
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee {EmployeeId}", id);
                }

                return new Response<Employee?>(
                updatedEmployee,
                true,
                "Updated Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while updating a employee: {Message}", sqlEx.Message);
                return new Response<Employee?>(
                null,
                false,
                "A database error occurred while updating the employee.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating a employee in repository: {Message}", ex.Message);
                return new Response<Employee?>(
                null,
                false,
                "A database error occurred while updating the employee in repository.",
                "500");
            }
        }

        public async Task<Response<Employee?>> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            con.Open();

            try
            {
                var entityToDelete = await GetByIdAsync(id);
                if (entityToDelete == null) return null;

                var deletedEmployee = await con.QuerySingleOrDefaultAsync<Employee>(
                    "dbo.DeleteEmployee",
                    new { id },
                    commandType: CommandType.StoredProcedure);

                if (deletedEmployee == null)
                {
                    return new Response<Employee?>(
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
                        OperationType = "Delete",
                        EntityName = "Employee",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateDeleteDetails(deletedEmployee)
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch(Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee {EmployeeId}", id);
                }

                return new Response<Employee?>(
                    deletedEmployee,
                    true,
                    "Deleted Successfully",
                    "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while deleting a employee: {Message}", sqlEx.Message);
                return new Response<Employee?>(
                null,
                false,
                "A database error occurred while deleting the employee.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a employee in repository: {Message}", ex.Message);
                return new Response<Employee?>(
                null,
                false,
                "A database error occurred while deleting the employee in repository.",
                "500");
            }
        }

        public async Task<Response<IEnumerable<Employee?>>> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            con.Open();
            try
            {
                var result = await con.QueryAsync<Employee>("dbo.GetAllEmployees", commandType: CommandType.StoredProcedure);
                return new Response<IEnumerable<Employee?>>(result, true, "Fetched all Employees successfully!", "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching Employee in repository: {Message}", sqlEx.Message);
                return new Response<IEnumerable<Employee?>>(
                    null,
                    false,
                    "A database error occurred while fetching Employee in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching Employee in repository: {Message}", ex.Message);
                return new Response<IEnumerable<Employee?>>(
                    null,
                    false,
                    "An unexpected error occurred while fetching Employee in repository.",
                    "500"
                );
            }
        }

        public async Task<Response<Employee?>> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            con.Open();
            try
            {
                var result = await con.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { id }, commandType: CommandType.StoredProcedure);
                return new Response<Employee?>(
                    result, true, "Fetched the employee successfully","200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching employee in repository: {Message}", sqlEx.Message);
                return new Response<Employee?>(
                    null,
                    false,
                    "A database error occurred while fetching Employee in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching Employee in repository: {Message}", ex.Message);
                return new Response<Employee?>(
                    null,
                    false,
                    "An unexpected error occurred while fetching Employee in repository.",
                    "500"
                );
            }

        }
    }
}
