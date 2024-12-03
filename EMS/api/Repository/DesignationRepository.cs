using System.Data;
using System.Transactions;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace api.Repository
{
    public class DesignationRepository(IFactoryDbContext context, IOperationLogService operationLogService, ILogger<DesignationRepository> logger) : IDesignationRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<DesignationRepository> _logger = logger;

        public async Task<Response<Designation?>> AddAsync(Designation entity)
        {
            using var con = _context.SqlConnection();
            using var transaction = con.BeginTransaction();

            try
            {
                var designation = await con.QuerySingleOrDefaultAsync<Designation>(
                    "dbo.AddDesignation",
                    new { entity.Role },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (designation == null)
                {
                    transaction.Rollback();
                    return new Response<Designation?>(
                        null,
                        false,
                        "Failed to add Designation from Repositry.",
                        "404"
                    );
                }

                var log = new OperationLog
                {
                    OperationType = "Insert",
                    EntityName = "Designation",
                    EntityId = designation.Id,
                    TimeStamp = DateTime.UtcNow,
                    OperationDetails = OperationLogHelper.CreateInsertDetails(designation)
                };

                await _operationLogService.LogOperationAsync(log);

                transaction.Commit();


                return new Response<Designation?>(
                designation,
                true,
                "Added Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                _logger.LogError(sqlEx, "SQL error occurred while adding a Designation: {Message}", sqlEx.Message);
                return new Response<Designation?>(
                null,
                false,
                "A database error occurred while adding the Designation.",
                "500");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Unexpected error occurred while adding a Designation in repository: {Message}", ex.Message);
                return new Response<Designation?>(
                null,
                false,
                "A database error occurred while adding the Designation in repository.",
                "500");
            }
        }

        public async Task<Response<Designation?>> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            using var transaction = con.BeginTransaction();

            try
            {
                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.DeleteDesignation",
                    new { id },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (designation == null)
                {
                    transaction.Rollback();
                    return new Response<Designation?>(
                        null,
                        false,
                        "Failed to Delete Designation from Repositry.",
                        "404"
                    );
                }

                var log = new OperationLog
                {
                    OperationType = "Delete",
                    EntityName = "Designation",
                    EntityId = designation.Id,
                    TimeStamp = DateTime.UtcNow,
                    OperationDetails = OperationLogHelper.CreateDeleteDetails(designation)
                };
                await _operationLogService.LogOperationAsync(log);
                transaction.Commit();


                return new Response<Designation?>(
                designation,
                true,
                "Deleted Successfully",
                "200");
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                _logger.LogError(sqlEx, "SQL error occurred while deleting a Designation: {Message}", sqlEx.Message);
                return new Response<Designation?>(
                null,
                false,
                "A database error occurred while deleting the Designation.",
                "500");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Unexpected error occurred while deleting a Designation in repository: {Message}", ex.Message);
                return new Response<Designation?>(
                null,
                false,
                "A database error occurred while deleting the Designation in repository.",
                "500");
            }
        }

        public async Task<Response<IEnumerable<Designation?>>> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QueryAsync<Designation>("dbo.GetAllDesignations", commandType: CommandType.StoredProcedure);
                return new Response<IEnumerable<Designation?>>(
                result,
                true,
                "Fetched all departments successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching a Designation: {Message}", sqlEx.Message);
                return new Response<IEnumerable<Designation?>>(
                null,
                false,
                "A database error occurred while fetching the Designation.",
                "500");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching a Designation in repository: {Message}", ex.Message);
                return new Response<IEnumerable<Designation?>>(
                null,
                false,
                "A database error occurred while fetching the Designation in repository.",
                "500");
            }

        }

        public async Task<Response<Designation?>> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QueryFirstOrDefaultAsync<Designation>("dbo.GetDesignationById", new { id }, commandType: CommandType.StoredProcedure);
                return new Response<Designation?>(
                result,
                true,
                "Fetched the Designation successfully.",
                "200");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred while fetching Designation in repository: {Message}", sqlEx.Message);
                return new Response<Designation?>(
                    null,
                    false,
                    "A database error occurred while fetching Designation in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching Designation in repository: {Message}", ex.Message);
                return new Response<Designation?>(
                    null,
                    false,
                    "An unexpected error occurred while fetching Designation in repository.",
                    "500"
                );
            }
        }

        public async Task<Response<Designation?>> UpdateAsync(int id, Designation entity)
        {
            using var con = _context.SqlConnection();
            using var transaction = con.BeginTransaction();

            try
            {
                var oldEntity = await GetByIdAsync(id);
                if (oldEntity.Result == null)
                {
                    transaction.Rollback();
                    return new Response<Designation?>(
                        null,
                        false,
                        "Designation not found for update in repository.",
                        "404"
                    );
                }
                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.UpdateDesignation",
                    new { id, entity.Role },
                    transaction,
                    commandType: CommandType.StoredProcedure);



                var log = new OperationLog
                {
                    OperationType = "Update",
                    EntityName = "Designation",
                    EntityId = designation!.Id,
                    TimeStamp = DateTime.UtcNow,
                    OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity.Result, designation)
                };
                await _operationLogService.LogOperationAsync(log);
                transaction.Commit();


                return new Response<Designation?>(
                    designation,
                    true,
                    "Updated successfully.",
                    "200"
                );
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                _logger.LogError(sqlEx, "SQL error occurred while updating Designation in repository: {Message}", sqlEx.Message);
                return new Response<Designation?>(
                    null,
                    false,
                    "A database error occurred while updating the Designation in repository.",
                    "500"
                );
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Unexpected error occurred while updating Designation in repository: {Message}", ex.Message);
                return new Response<Designation?>(
                    null,
                    false,
                    "An unexpected error occurred while updating the Designation in repository.",
                    "500"
                );
            }
        }
    }
}
