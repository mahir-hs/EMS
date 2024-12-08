using System.Data;
using System.Transactions;
using System.Transactions;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace api.Repository
{
    public class DesignationRepository(IFactoryDbContext context, IOperationLogService operationLogService, ILogger<DesignationRepository> logger) : IDesignationRepository
    public class DesignationRepository(IFactoryDbContext context, IOperationLogService operationLogService, ILogger<DesignationRepository> logger) : IDesignationRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<DesignationRepository> _logger = logger;
        private readonly ILogger<DesignationRepository> _logger = logger;

        public async Task<ApiResponse> AddAsync(Designation entity)
        {
            using var con = _context.SqlConnection();
            try
            {
                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.AddDesignation",
                    new { entity.Role },
                    commandType: CommandType.StoredProcedure);
                
                if (designation == null)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Failed to add Designation from Repository.",
                        "500"
                    );
                }

                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Designation",
                        EntityId = designation.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = $"Added Designation: Id={designation.Id}, Role={designation.Role}"
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on Designation {DesignationId}", designation.Id);
                }
                

                return new ApiResponse(
                designation,
                true,
                "Added Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while adding a Designation: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while adding the Designation.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a Designation in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while adding the Designation in repository.",
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

                await con.QuerySingleOrDefaultAsync<Designation>(
                    "dbo.DeleteDesignation",
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
                        OperationType = "Delete",
                        EntityName = "Designation",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = $"Deleted Designation: Id={id}"
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error occurred while deleting a Designation in repository: {Message}", ex.Message);
                }
                return new ApiResponse(
                null,
                true,
                "Deleted Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while deleting a Designation: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while deleting the Designation.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a Designation in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while deleting the Designation in repository.",
                "500");
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QueryAsync<Designation>("dbo.GetAllDesignations", commandType: CommandType.StoredProcedure);
                return new ApiResponse(
                result,
                true,
                "Fetched all departments successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching a Designation: {Message}", sqlEx.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while fetching the Designation.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching a Designation in repository: {Message}", ex.Message);
                return new ApiResponse(
                null,
                false,
                "A database error occurred while fetching the Designation in repository.",
                "500");
            }

        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QuerySingleOrDefaultAsync<Designation>("dbo.GetDesignationById", new { id }, commandType: CommandType.StoredProcedure);
                return new ApiResponse(
                result,
                true,
                "Fetched the Designation successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching Designation in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while fetching Designation in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching Designation in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while fetching Designation in repository.",
                    "500"
                );
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, Designation entity)
        {
            using var con = _context.SqlConnection();
            try
            {
                var oldEntity = await GetByIdAsync(id);
                if (!oldEntity.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        oldEntity.Message,
                        oldEntity.Type
                    );
                }
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                parameters.Add("Role", entity.Role);
                parameters.Add("ErrorMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.UpdateDesignation",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var errorMessage = parameters.Get<string>("ErrorMessage");
                var result = parameters.Get<int>("ReturnValue");

                if(result==-1)
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
                        EntityName = "Designation",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity.Result, designation)
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error occurred while updating Designation in repository: {Message}", ex.Message);
                }
               

                return new ApiResponse(
                    designation,
                    true,
                    "Updated successfully.",
                    "200"
                );
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while updating Designation in repository: {Message}", sqlEx.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while updating the Designation in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating Designation in repository: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while updating the Designation in repository.",
                    "500"
                );
            }
        }
    }
}
