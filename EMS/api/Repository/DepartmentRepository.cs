using System.Data;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Azure;
using Dapper;
using Microsoft.Data.SqlClient;

namespace api.Repository
{
    public class DepartmentRepository(IFactoryDbContext context, IOperationLogService operationLogService,ILogger<DepartmentRepository> logger) : IDepartmentRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<DepartmentRepository> _logger = logger;

        public async Task<ApiResponse> AddAsync(Department entity)
        {
            using var con = _context.SqlConnection();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("Dept", entity.Dept);
                parameters.Add("ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await con.ExecuteScalarAsync<int>(
                    "dbo.AddDepartment",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var result = parameters.Get<int>("ReturnValue");
                var errorMessage = parameters.Get<string>("ErrorMessage");
                var id = parameters.Get<int?>("Id");
               
                if (result==1)
                {
                    try
                    {
                        var log = new OperationLog
                        {
                            OperationType = "Insert",
                            EntityName = "Department",
                            EntityId = id!.Value,
                            TimeStamp = DateTime.UtcNow,
                            OperationDetails = $"Added Department: Id={id.Value}, Dept={entity.Dept}"
                        };

                        await _operationLogService.LogOperationAsync(log);
                       
                    }
                    catch (Exception logEx)
                    {
                        _logger.LogError(logEx, "Error logging operation for department: {Message}", logEx.Message);
                    }
                    entity.Id = id!.Value;
                    return new ApiResponse(
                        entity,
                        true,
                        "Department added successfully.",
                        "200"
                    );

                }

               
                return new ApiResponse(
                    null,
                    false,
                    errorMessage,
                    "500"
                );
                

            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while adding a department: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while adding the department.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a department in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while adding the department in repository.",
                "500");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                parameters.Add("ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await con.ExecuteScalarAsync<int>(
                    "dbo.DeleteDepartment",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var errorMessage = parameters.Get<string>("ErrorMessage");
                var result = parameters.Get<int>("ReturnValue");

                if (result == 1)
                {
                    


                    try
                    {
                        var log = new OperationLog
                        {
                            OperationType = "Delete",
                            EntityName = "Department",
                            EntityId = id,
                            TimeStamp = DateTime.UtcNow,
                            OperationDetails = $"Deleted Department: Id={id}"
                        };

                        await _operationLogService.LogOperationAsync(log);
                    }
                    catch (Exception ex) {
                        _logger.LogError(ex, "Error logging operation for department: {Message}", ex.Message);
                    }
                    return new ApiResponse(
                        true,
                        true,
                        "Department deleted successfully.",
                        "200"
                    );
                }

                return new ApiResponse(
                    false,
                    false,
                    errorMessage ?? "Failed to delete the department.",
                    "500"
                );
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while deleting a department in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while deleting the department in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a department in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while deleting the department in repository.",
                    "500"
                );
            }
        }


        public async Task<ApiResponse> GetAllAsync()
        {
            using var con = _context.SqlConnection();

            try
            {
                var result = await con.QueryAsync<Department>("dbo.GetAllDepartments", commandType: CommandType.StoredProcedure);

                return new ApiResponse(
                result,
                true,
                "Fetched all departments successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching departments in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while fetching departments in repository.",
                    "500"
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching departments in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while fetching departments in repository.",
                    "500"
                );
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QuerySingleOrDefaultAsync<Department>("dbo.GetDepartmentById", new { id }, commandType: CommandType.StoredProcedure);
                return new ApiResponse(
                result,
                true,
                "Fetched the department successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching department in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while fetching department in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching departments in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while fetching department in repository.",
                    "500"
                );
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, Department entity)
        {
            using var con = _context.SqlConnection();

            try
            {
                var oldEntityResponse = await GetByIdAsync(id);
                var oldEntity = oldEntityResponse?.Result;

                if (!oldEntityResponse!.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        oldEntityResponse.Message,
                        oldEntityResponse.Type
                    );
                }
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                parameters.Add("Dept", entity.Dept);
                parameters.Add("ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var department = await con.QuerySingleOrDefaultAsync<Department>(
                    "dbo.UpdateDepartment",
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
                        EntityName = "Department",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, department)
                    };

                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception ex) {  _logger.LogError(ex, "Error logging operation for department: {Message}", ex.Message);
                }

                return new ApiResponse(
                    department,
                    true,
                    "Updated successfully.",
                    "200"
                );
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while updating department in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while updating the department in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating department in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while updating the department in repository.",
                    "500"
                );
            }
        }
    }
}
