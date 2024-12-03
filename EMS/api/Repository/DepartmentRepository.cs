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
    public class DepartmentRepository(IFactoryDbContext context, IOperationLogService operationLogService,ILogger<DepartmentRepository> logger) : IDepartmentRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<DepartmentRepository> _logger = logger;

        public async Task<Response<Department?>> AddAsync(Department entity)
        {
            using var con = _context.SqlConnection();
            using var transaction = con.BeginTransaction();


            try
            {   
                var department = await con.QuerySingleOrDefaultAsync<Department>(
                    "dbo.AddDepartment",
                    new { entity.Dept },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                
                if (department == null)
                {
                    transaction.Rollback();
                    return new Response<Department?>(
                        null,
                        false,
                        "Failed to add department.",
                        "500"
                    );
                }

                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Department",
                        EntityId = department.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateInsertDetails(department)
                    };

                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on department {DepartmentId}", department.Id);
                }
                transaction.Commit();


                return new Response<Department?>(
                department,
                true,
                "Added Successfully",
                "200");

            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                _logger.LogError(sqlEx, "SQL error occurred while adding a department: {Message}", sqlEx.Message);
                return new Response<Department?>(
                null,
                false,
                "A database error occurred while adding the department.",
                "500");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Unexpected error occurred while adding a department in repository: {Message}", ex.Message);
                return new Response<Department?>(
                null,
                false,
                "A database error occurred while adding the department in repository.",
                "500");
            }
        }

        public async Task<Response<Department?>> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            using var transaction = con.BeginTransaction();

            try
            {
                var department = await con.QuerySingleOrDefaultAsync<Department>(
                    "dbo.DeleteDepartment",
                    new { id },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (department == null)
                {
                    transaction.Rollback();
                    return new Response<Department?>(
                        null,
                        false,
                        "Department not found. Deletion failed in repository.",
                        "404"
                    );
                }

               
                var log = new OperationLog
                {
                    OperationType = "Delete",
                    EntityName = "Department",
                    EntityId = department.Id,
                    TimeStamp = DateTime.UtcNow,
                    OperationDetails = OperationLogHelper.CreateDeleteDetails(department)
                };

                await _operationLogService.LogOperationAsync(log);
                transaction.Commit();


                return new Response<Department?>(
                department,
                true,
                "Deleted Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                _logger.LogError(sqlEx, "SQL error occurred while deleting a department in repository: {Message}", sqlEx.Message);
                return new Response<Department?>(
                    null,
                    false,
                    "A database error occurred while deleting the department in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Unexpected error occurred while deleting a department in repository: {Message}", ex.Message);
                return new Response<Department?>(
                    null,
                    false,
                    "An unexpected error occurred while deleting the department in repository.",
                    "500"
                );
            }
        }


        public async Task<Response<IEnumerable<Department?>>> GetAllAsync()
        {
            using var con = _context.SqlConnection();

            try
            {
                var result = await con.QueryAsync<Department>("dbo.GetAllDepartments", commandType: CommandType.StoredProcedure);

                return new Response<IEnumerable<Department?>>(
                result,
                true,
                "Fetched all departments successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching departments in repository: {Message}", sqlEx.Message);
                return new Response<IEnumerable<Department?>>(
                    null,
                    false,
                    "A database error occurred while fetching departments in repository.",
                    "500"
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching departments in repository: {Message}", ex.Message);
                return new Response<IEnumerable<Department?>>(
                    null,
                    false,
                    "An unexpected error occurred while fetching departments in repository.",
                    "500"
                );
            }
        }

        public async Task<Response<Department?>> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QueryFirstOrDefaultAsync<Department>("dbo.GetDepartmentById", new { id }, commandType: CommandType.StoredProcedure);
                return new Response<Department?>(
                result,
                true,
                "Fetched the department successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching department in repository: {Message}", sqlEx.Message);
                return new Response<Department?>(
                    null,
                    false,
                    "A database error occurred while fetching department in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching departments in repository: {Message}", ex.Message);
                return new Response<Department?>(
                    null,
                    false,
                    "An unexpected error occurred while fetching department in repository.",
                    "500"
                );
            }
        }

        public async Task<Response<Department?>> UpdateAsync(int id, Department entity)
        {
            using var con = _context.SqlConnection();

            try
            {
                var oldEntityResponse = await GetByIdAsync(id);
                var oldEntity = oldEntityResponse?.Result;

                if (oldEntity == null)
                {
                    return new Response<Department?>(
                        null,
                        false,
                        "Department not found for update in repository.",
                        "404"
                    );
                }

                var department = await con.QuerySingleOrDefaultAsync<Department>(
                    "dbo.UpdateDepartment",
                    new { id, entity.Dept },
                    commandType: CommandType.StoredProcedure);

                if (department == null)
                {
                    return new Response<Department?>(
                        null,
                        false,
                        "Failed to update the department in repository.",
                        "500"
                    );
                }

                var log = new OperationLog
                {
                    OperationType = "Update",
                    EntityName = "Department",
                    EntityId = department.Id,
                    TimeStamp = DateTime.UtcNow,
                    OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, department)
                };

                await _operationLogService.LogOperationAsync(log);

                return new Response<Department?>(
                    department,
                    true,
                    "Updated successfully.",
                    "200"
                );
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while updating department in repository: {Message}", sqlEx.Message);
                return new Response<Department?>(
                    null,
                    false,
                    "A database error occurred while updating the department in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating department in repository: {Message}", ex.Message);
                return new Response<Department?>(
                    null,
                    false,
                    "An unexpected error occurred while updating the department in repository.",
                    "500"
                );
            }
        }
    }
}
