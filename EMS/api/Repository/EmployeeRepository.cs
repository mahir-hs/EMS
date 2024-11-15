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
    public class EmployeeRepository(IDapperContext context, IOperationLogService operationLogService) : IEmployeeRepository
    {
        private readonly IDapperContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;

        public async Task<Employee?> AddAsync(Employee entity)
        {
            using var con = _context.CreateConnection();
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
                    var logDetails = OperationLogHelper.CreateInsertDetails(employee);
                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Employee",
                        EntityId = employee.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
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

        public async Task<Employee?> UpdateAsync(Employee entity)
        {
            using var con = _context.CreateConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            try
            {
                var oldEntity = await GetByIdAsync(entity.Id);
                if (oldEntity == null) return null;

                var updatedEmployee = await con.QueryFirstOrDefaultAsync<Employee>(
                    "dbo.UpdateEmployee",
                    new
                    {
                        entity.Id,
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
                    var logDetails = OperationLogHelper.CreateUpdateDetails(oldEntity, updatedEmployee);
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Employee",
                        EntityId = updatedEmployee.Id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
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
            using var con = _context.CreateConnection();
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
                    var logDetails = OperationLogHelper.CreateDeleteDetails(deletedEmployee);

                    var log = new OperationLog
                    {
                        OperationType = "Delete",
                        EntityName = "Employee",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = logDetails
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
            using var con = _context.CreateConnection();
            return await con.QueryAsync<Employee>("dbo.GetAllEmployees", commandType: CommandType.StoredProcedure);
        }

        public async Task<Employee?> GetByIdAsync(int Id)
        {
            using var con = _context.CreateConnection();
            return await con.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { Id }, commandType: CommandType.StoredProcedure);
        }
    }
}
