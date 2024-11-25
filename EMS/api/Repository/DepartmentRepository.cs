using System.Data;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;

namespace api.Repository
{
    public class DepartmentRepository(IFactoryDbContext context, IOperationLogService operationLogService) : IDepartmentRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;

        public async Task<Department?> AddAsync(Department entity)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var department = await con.QueryFirstOrDefaultAsync<Department>(
                    "dbo.AddDepartment",
                    new { entity.Dept },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (department != null)
                {
                    var logDetails = OperationLogHelper.CreateInsertDetails(department);

                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Department",
                        EntityId = department.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
                    };
                    await _operationLogService.LogOperationAsync(log);
                    transaction.Commit();
                }

                return department;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Department?> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var department = await con.QueryFirstOrDefaultAsync<Department>(
                    "dbo.DeleteDepartment",
                    new { id },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (department != null)
                {
                    var logDetails = OperationLogHelper.CreateDeleteDetails(department);
                    var log = new OperationLog
                    {
                        OperationType = "Delete",
                        EntityName = "Department",
                        EntityId = department.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
                    };

                    await _operationLogService.LogOperationAsync(log);
                    transaction.Commit();
                }

                return department;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Department?>> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            return await con.QueryAsync<Department>("dbo.GetAllDepartments", commandType: CommandType.StoredProcedure);
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            using var con = _context.SqlConnection();
            return await con.QueryFirstOrDefaultAsync<Department>("dbo.GetDepartmentById", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Department?> UpdateAsync(int id, Department entity)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var oldEntity = await GetByIdAsync(id);
                var department = await con.QueryFirstOrDefaultAsync<Department>(
                    "dbo.UpdateDepartment",
                    new { id, entity.Dept },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (department != null)
                {
                    var logDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, department);
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Department",
                        EntityId = department.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
                    };
                    await _operationLogService.LogOperationAsync(log);
                    transaction.Commit();
                }

                return department;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
