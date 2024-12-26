using api.Dto.Account;
using api.Dto.Identity;
using api.Models;

namespace api.Services.IServices
{
    public interface IAccountService
    {
        Task<ApiResponse> Login(LoginDto loginDto);
        Task<ApiResponse> Register(RegisterDto registerDto);

        Task<ApiResponse> RefreshToken(TokenApiDto tokenApiDto);
        
        Task<ApiResponse> LogOut(string accessToken);

        Task<ApiResponse> ChangePassword(ChangePasswordDto changePasswordDto);

        Task<ApiResponse> ChangeEmail(ChangeEmailDto changeEmailDto);
    }
}
