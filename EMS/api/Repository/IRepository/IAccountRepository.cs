using api.Dto.Account;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<ApiResponse> AddAsync(Account entity);
        Task<ApiResponse> UpdateAsync(Account account);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> RefreshTokenExists(string refreshToken);
        Task<ApiResponse> GetUserData(string email);
        Task<ApiResponse> LogOut(string email);


    }
}
