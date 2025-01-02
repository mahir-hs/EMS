using api.Dto.Account;
using api.Dto.Identity;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class AccountService(IAccountRepository context, IConfiguration config, ILogger<AccountService> logger, IEmailService emailService,IJwtService jwt) : IAccountService
    {
        private readonly IAccountRepository _context = context;
        private readonly IConfiguration _config = config;
        private readonly ILogger<AccountService> _logger = logger;
        private readonly IEmailService _emailService = emailService;
        private readonly IJwtService _jwt = jwt;
        public async Task<ApiResponse> Login(LoginDto loginDto)
        {
            try
            {
                var data = await _context.GetUserData(loginDto.Email);
                if (!data.Success)
                {
                    return data;
                }

                var user = data.Result as Account;
                
                if ( !PasswordHasher.VerifyPassword( loginDto.Password, user!.Password) )
                {
                    return new ApiResponse(null, false, "Invalid password.");
                }

                user!.Token = _jwt.CreateJwt(user.Email,user.RoleName);
                var newAccessToken = user.Token;
                var newRefreshToken = await _jwt.CreateRefreshToken();
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5);
                Console.WriteLine($"User: {user} {user.Email} {user.Password} {user.RoleId} {user.Token} {user.RefreshToken} {user.RefreshTokenExpiryTime}");
                await _context.UpdateAsync(user);

                return new ApiResponse(new TokenApiDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken }, true, "Login successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in.");
                return new ApiResponse(new { error = ex.ToString() });
            }

        }

        public async Task<ApiResponse> LogOut(string accessToken)
        {
            try
            {
                var principal = _jwt.GetPrincipleFromExpiredToken(accessToken);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");
                Console.WriteLine(emailClaim);
                var result = await _context.LogOut(emailClaim!.Value);
                if (!result.Success)
                {
                    return result;
                }

                return new ApiResponse(null, true, "Logout successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging out.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }

        public async Task<ApiResponse> Register(RegisterDto registerDto, int roleId)
        {
            try
            {
                var checkPass = PasswordValidator.CheckPassword(registerDto.Password);
                if (checkPass.Length > 0)
                {
                    return new ApiResponse(checkPass, false, "Password is not valid.", "400");
                }
                var passHash = PasswordHasher.HashPassword(registerDto.Password);
                var role = (roleId == 1) ? "Admin" : "User";
                var user = new Account
                {
                    Email = registerDto.Email,
                    Password = passHash,
                    RoleId = roleId,
                    Token = _jwt.CreateJwt(registerDto.Email, role),
                    RefreshToken = await _jwt.CreateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
                };
                Console.WriteLine($"User: {user} {user.Email} {user.Password} {user.RoleId} {user.Token} {user.RefreshToken} {user.RefreshTokenExpiryTime}");
                var response = await _context.AddAsync(user);
                if (!response.Success)
                {
                    return response;
                }
                return new ApiResponse(user, true, "User registered successfully.");


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }

        

        public async Task<ApiResponse> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var principal = _jwt.GetPrincipleFromExpiredToken(changePasswordDto.Token);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");
                var response = await _context.GetUserData(emailClaim!.Value);
                if (!response.Success)
                {
                    return response;
                }
                var user = response.Result as Account;
                var checkPass = PasswordValidator.CheckPassword(changePasswordDto.NewPassword);
                if (checkPass.Length > 0)
                {
                    return new ApiResponse(checkPass, false, "Password is not valid.", "400");
                }
                
                var passHash = PasswordHasher.HashPassword(changePasswordDto.NewPassword);
                user!.Password = passHash;
                await _context.UpdateAsync(user);
                return new ApiResponse(user, true, "Password changed successfully.");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while changing password.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }

        public async Task<ApiResponse> ChangeEmail(ChangeEmailDto changeEmailDto)
        {
            try
            {
                var principal = _jwt.GetPrincipleFromExpiredToken(changeEmailDto.Token);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");
                var response = await _context.GetUserData(emailClaim!.Value);
                if (!response.Success)
                {
                    return response;
                }
                var user = response.Result as Account;
                var checkPass = PasswordHasher.VerifyPassword(changeEmailDto.Password, user!.Password);
                if (!checkPass) {
                    return new ApiResponse(null, false, "Password is incorrect.", "400");
                }

                var emailExists = await _context.GetUserData(changeEmailDto.NewEmail);
                if (emailExists.Success)
                {
                    return new ApiResponse(null, false, "Email already exists.", "400");
                }

                user!.Email = changeEmailDto.NewEmail;
                user.Token = _jwt.CreateJwt(user.Email,user.RoleName);
                await _context.UpdateAsync(user);
                return new ApiResponse(user, true, "Email changed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while changing email.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }





        

        public async Task<ApiResponse> RequestPasswordReset(RequestPasswordResetDto requestPasswordReset)
        {
            try
            {
                var userResponse = await _context.GetUserData(requestPasswordReset.Email);
                if (!userResponse.Success)
                {
                    return new ApiResponse(null, false, "Email address not found.","202");
                }

                var resetToken = _jwt.CreateJwt(requestPasswordReset.Email, userResponse.Result!.RoleName);

                var resetLink = $"{_config["App:FrontendBaseUrl"]}/reset-password?token={resetToken}";

               
                var emailSent = await _emailService.SendEmailAsync(
                   requestPasswordReset.Email,
                    "Password Reset Request",
                    $"Click <a href='{resetLink}'>here</a> to reset your password. The link will expire in 5 minutes."
                );

                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send password reset email to {Email}", requestPasswordReset.Email);
                }

                return new ApiResponse(null, true, "Password reset email sent.","202");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while requesting password reset.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }

        public async Task<ApiResponse> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var principal = _jwt.GetPrincipleFromExpiredToken(resetPasswordDto.Token);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");
                if (emailClaim == null)
                {
                    return new ApiResponse(null, false, "Invalid reset token.");
                }

                var response = await _context.GetUserData(emailClaim.Value);
                if (!response.Success)
                {
                    return response;
                }
                var user = response.Result as Account;
                string validPass = PasswordValidator.CheckPassword(resetPasswordDto.Password);
                if (validPass.Length > 0 )
                {
                    return new ApiResponse(validPass, false, "Password must be at least 8 characters long.");
                }
                var passHash = PasswordHasher.HashPassword(resetPasswordDto.Password);
                user!.Password = passHash;
                await _context.UpdateAsync(user);

                var emailSent = await _emailService.SendEmailAsync(
                   emailClaim.Value,
                    "Password Reset Successfully",
                    $"Your password has been successfully reset. Click <a href='{_config["App:FrontendBaseUrl"]}/login'>here</a> to login."
                );

                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send success password reset email to {Email}", emailClaim.Value);
                }

                return new ApiResponse(user, true, "Password has been successfully reset.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while resetting password.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }
    }
}
