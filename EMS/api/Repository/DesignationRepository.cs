using System.Data;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;

namespace api.Repository
{
    public class DesignationRepository(IDapperContext context, IOperationLogService operationLogService) : IDesignationRepository
    {
        private readonly IDapperContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;

        public async Task<Designation?> AddAsync(Designation entity)
        {
            using var con = _context.CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var designation = await con.QuerySingleOrDefaultAsync<Designation>(
                    "dbo.AddDesignation",
                    new { entity.Role },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (designation != null)
                {
                    var logDetails = OperationLogHelper.CreateInsertDetails(designation);
                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Designation",
                        EntityId = designation.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
                    };

                    await _operationLogService.LogOperationAsync(log);

                    transaction.Commit();
                }

                return designation;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Designation?> DeleteAsync(int id)
        {
            using var con = _context.CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.DeleteDesignation",
                    new { id },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (designation != null)
                {
                    var logDetails = OperationLogHelper.CreateDeleteDetails(designation);
                    var log = new OperationLog
                    {
                        OperationType = "Delete",
                        EntityName = "Designation",
                        EntityId = designation.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
                    };
                    await _operationLogService.LogOperationAsync(log);
                    transaction.Commit();
                }

                return designation;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Designation?>> GetAllAsync()
        {
            using var con = _context.CreateConnection();
            return await con.QueryAsync<Designation>("dbo.GetAllDesignations", commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> GetByIdAsync(int id)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Designation>("dbo.GetDesignationById", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> UpdateAsync(Designation entity)
        {
            using var con = _context.CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var oldEntity = await GetByIdAsync(entity.Id);
                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.UpdateDesignation",
                    new { entity.Id, entity.Role },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (designation != null)
                {
                    var logDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, designation);
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Designation",
                        EntityId = designation.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
                    };
                    await _operationLogService.LogOperationAsync(log);
                    transaction.Commit();
                }

                return designation;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
