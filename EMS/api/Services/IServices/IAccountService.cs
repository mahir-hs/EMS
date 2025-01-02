using api.Dto.Account;
using api.Dto.Identity;
using api.Models;

namespace api.Services.IServices
{
    public interface IAccountService
    {
        Task<ApiResponse> Login(LoginDto loginDto);
        Task<ApiResponse> Register(RegisterDto registerDto,int roleId);
        
        Task<ApiResponse> LogOut(string accessToken);

        Task<ApiResponse> ChangePassword(ChangePasswordDto changePasswordDto);

        Task<ApiResponse> ChangeEmail(ChangeEmailDto changeEmailDto);

        Task<ApiResponse> RequestPasswordReset(RequestPasswordResetDto requestPasswordReset);

        Task<ApiResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
