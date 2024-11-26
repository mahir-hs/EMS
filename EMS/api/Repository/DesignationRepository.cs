using System.Data;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;

namespace api.Repository
{
    public class DesignationRepository(IFactoryDbContext context, IOperationLogService operationLogService) : IDesignationRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;

        public async Task<Designation?> AddAsync(Designation entity)
        {
            using var con = _context.SqlConnection();
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
            using var con = _context.SqlConnection();
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
            using var con = _context.SqlConnection();
            return await con.QueryAsync<Designation>("dbo.GetAllDesignations", commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            return await con.QueryFirstOrDefaultAsync<Designation>("dbo.GetDesignationById", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> UpdateAsync(int id, Designation entity)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var oldEntity = await GetByIdAsync(id);
                var designation = await con.QueryFirstOrDefaultAsync<Designation>(
                    "dbo.UpdateDesignation",
                    new { id, entity.Role },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (designation != null)
                {
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Designation",
                        EntityId = designation.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, designation)
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
