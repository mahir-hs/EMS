using api.Data.Contexts;
using api.Dto.Account;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;
using System.Data;
using System.Data.Common;

namespace api.Repository
{
    public class AccountRepository(IFactoryDbContext context, IOperationLogService operationLogService, ILogger<DepartmentRepository> logger) : IAccountRepository
    {
        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<DepartmentRepository> _logger = logger;


        public async Task<ApiResponse> AddAsync(Account entity)
        {
            using var con = _context.SqlConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Email", entity.Email);
                parameters.Add("@Password", entity.Password);
                parameters.Add("@RoleId", entity.RoleId);
                parameters.Add("@Token",entity.Token);
                parameters.Add("@RoleName", entity.RoleName);
                parameters.Add("@RefreshToken", entity.RefreshToken);
                parameters.Add("@RefreshTokenExpiryTime", entity.RefreshTokenExpiryTime);
                parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await con.ExecuteScalarAsync<int>(
                    "dbo.AddNewUser",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var result = parameters.Get<int>("ReturnValue");
                var errorMessage = parameters.Get<string>("ErrorMessage");
                var id = parameters.Get<int?>("Id");

                if (result == 1)
                {
                    try
                    {
                        var log = new OperationLog
                        {
                            OperationType = "Insert",
                            EntityName = "UserAccount",
                            EntityId = id!.Value,
                            TimeStamp = DateTime.UtcNow,
                            OperationDetails = $"Added Account: Id={id.Value}, Email={entity.Email}"
                        };

                        await _operationLogService.LogOperationAsync(log);

                    }
                    catch (Exception logEx)
                    {
                        _logger.LogError(logEx, "Error logging operation for Account Creation: {Message}", logEx.Message);
                    }
                    entity.Id = id!.Value;
                    return new ApiResponse(
                        entity,
                        true,
                        "Account added successfully.",
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
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while adding account: {Message}", ex.Message);
                return new ApiResponse(
               null,
               false,
               "A database error occurred while adding the account in repository.",
               "500");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            using var con = _context.SqlConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                await con.ExecuteScalarAsync<int>(
                    "dbo.DeleteAccount",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new ApiResponse(
                    null,
                    true,
                    "Account deleted successfully.",
                    "200"
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting account: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while deleting the account in repository.",
                    "500");
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            using var con = _context.SqlConnection();
            try
            {
                var result = await con.QueryAsync<Account>("dbo.GetAllAccounts", commandType: CommandType.StoredProcedure);
                return new ApiResponse(
                    result,
                    true,
                    "Fetched all accounts successfully.",
                    "200"
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching accounts: {Message}", ex.Message);
                return new ApiResponse(
                    null,
                    false,
                    "A database error occurred while fetching accounts in repository.",
                    "500");
            }
        }

        //public async Task<ApiResponse> GetAsync(LoginDto entity)
        //{
        //    using var con = _context.SqlConnection();
        //    try
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@Email", entity.Email);
        //        parameters.Add("@Password", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
        //        parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
        //        parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        //        await con.ExecuteScalarAsync("dbo.GetUserByEmail", parameters, commandType: CommandType.StoredProcedure);

        //        var errorMessage = parameters.Get<string>("@ErrorMessage");
        //        var password = parameters.Get<string>("@Password");
        //        var result = parameters.Get<int>("ReturnValue");

        //        if (result == -1)
        //        {
        //            return new ApiResponse
        //            (
        //                null,
        //                false,
        //                errorMessage,
        //                "500"
        //            );
        //        }


        //        return new ApiResponse
        //        (
        //            new LoginDto
        //            {
        //                Email = entity.Email,
        //                Password = password
        //            },
        //            true,
        //            "User fetched successfully.",
        //            "200"
        //        );
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while fetching user: {Message}", ex.Message);
        //        return new ApiResponse(null, false, "An error occurred while fetching user.", "500");
        //    }
            
        //}

        public async Task<ApiResponse> GetUserData(string email)
        {
            using var con = _context.SqlConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                

                var fetch = await con.QuerySingleOrDefaultAsync<Account>("dbo.GetUserByEmail", parameters, commandType: CommandType.StoredProcedure);
                return new ApiResponse(
                    fetch,
                    true,
                    "Fetched user data successfully.",
                    "200"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while fetching user.", "500");
            }
        }

        public async Task<ApiResponse> LogOut(string email)
        {
            Console.WriteLine(email);
            using var con = _context.SqlConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await con.ExecuteScalarAsync("dbo.Logout", parameters, commandType: CommandType.StoredProcedure);
                
                var errorMessage = parameters.Get<string>("@ErrorMessage");
                var result = parameters.Get<int>("ReturnValue");
                if (result == -1)
                {
                    return new ApiResponse(
                        null,
                        false,
                        errorMessage,
                        "500"
                    );
                }
                return new ApiResponse( 
                    null,
                    true,
                    "Logged out successfully.",
                    "200"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while fetching user.", "500");
            }
        }

        public async Task<ApiResponse> RefreshTokenExists(string refreshToken)
        {
            using var con = _context.SqlConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RefreshToken", refreshToken);
                parameters.Add("@Exists", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await con.ExecuteScalarAsync("dbo.RefreshTokenExists", parameters, commandType: CommandType.StoredProcedure);

                var errorMessage = parameters.Get<string>("@ErrorMessage");
                var exists = parameters.Get<int>("@Exists");
                var result = parameters.Get<int>("ReturnValue");

                if (result == -1)
                {
                    return new ApiResponse
                    (
                        null,
                        false,
                        errorMessage,
                        "500"
                    );
                }

                return new ApiResponse
                (
                    exists,
                    true,
                    "Refresh token exists.",
                    "200"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while fetching user.", "500");
            }
        }

        public async Task<ApiResponse> UpdateAsync(Account account)
        {
            using var con = _context.SqlConnection();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", account.Id);
                parameters.Add("@Email", account.Email);
                parameters.Add("@Password", account.Password);
                parameters.Add("@RoleId", account.RoleId);
                parameters.Add("@RoleName", account.RoleName);
                parameters.Add("@Token", account.Token);
                parameters.Add("@RefreshToken", account.RefreshToken);
                parameters.Add("@RefreshTokenExpiryTime", account.RefreshTokenExpiryTime);

                var result = await con.QuerySingleOrDefaultAsync<Account>("dbo.UpdateUserAccount", parameters, commandType: CommandType.StoredProcedure);
                return new ApiResponse(result, true, "Updated account successfully.", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating account: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while updating account.", "500");
            }
            
        }
    }
}
