using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using api.Data.Contexts;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services;
using api.Services.IServices;
using Dapper;
using Microsoft.OpenApi.Models;

namespace api.Repository
{
    public class EmployeeRepository(IFactoryDbContext context, IOperationLogService operationLogService) : IEmployeeRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;

        public async Task<Employee?> AddAsync(Employee entity)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

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
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (employee != null)
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

                    transaction.Commit();
                }

                return employee;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Employee?> UpdateAsync(int id, Employee entity)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var oldEntity = await GetByIdAsync(id);
                if (oldEntity == null) return null;

                var updatedEmployee = await con.QueryFirstOrDefaultAsync<Employee>(
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
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (updatedEmployee != null)
                {
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Employee",
                        EntityId = updatedEmployee.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, updatedEmployee)
                    };
                    await _operationLogService.LogOperationAsync(log);
                    transaction.Commit();
                }

                return updatedEmployee;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Employee?> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var entityToDelete = await GetByIdAsync(id);
                if (entityToDelete == null) return null;

                var deletedEmployee = await con.QueryFirstOrDefaultAsync<Employee>(
                    "dbo.DeleteEmployee",
                    new { id },
                    transaction,
                    commandType: CommandType.StoredProcedure);

                if (deletedEmployee != null)
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
                    
                    transaction.Commit();
                }

                return deletedEmployee;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Employee?>> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            return await con.QueryAsync<Employee>("dbo.GetAllEmployees", commandType: CommandType.StoredProcedure);
        }

        public async Task<Employee?> GetByIdAsync(int Id)
        {
            using var con = _context.SqlConnection();
            return await con.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { Id }, commandType: CommandType.StoredProcedure);
        }
    }
}
