using api.Dto.Account;
using api.Dto.Identity;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api.Services
{
    public class AccountService(IAccountRepository context, IConfiguration config, ILogger<AccountService> logger) : IAccountService
    {
        private readonly IAccountRepository _context = context;
        private readonly IConfiguration _config = config;
        private readonly ILogger<AccountService> _logger = logger;
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

                user!.Token = CreateJwt(user.Email);
                var newAccessToken = user.Token;
                var newRefreshToken = await CreateRefreshToken();
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
                var principal = GetPrincipleFromExpiredToken(accessToken);
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

        public async Task<ApiResponse> Register(RegisterDto registerDto)
        {
            try
            {
                var checkPass = PasswordValidator.CheckPassword(registerDto.Password);
                if (checkPass.Length > 0)
                {
                    return new ApiResponse(checkPass, false, "Password is not valid.", "400");
                }
                var passHash = PasswordHasher.HashPassword(registerDto.Password);
                var user = new Account
                {
                    Email = registerDto.Email,
                    Password = passHash,
                    RoleId = registerDto.RoleId,
                    Token = CreateJwt(registerDto.Email),
                    RefreshToken = await CreateRefreshToken(),
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

        public async Task<ApiResponse> RefreshToken(TokenApiDto tokenApiDto)
        {
            try
            {
                var principal = GetPrincipleFromExpiredToken(tokenApiDto.AccessToken);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");
                var response = await _context.GetUserData(emailClaim!.Value);
                if (!response.Success)
                {
                    return response;
                }
                var user = response.Result as Account;
                if (user!.RefreshToken != tokenApiDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return new ApiResponse(new { error = "Invalid refresh token." });
                }
                user.Token = CreateJwt(emailClaim!.Value);
                user.RefreshToken = await CreateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1);
                await _context.UpdateAsync(user);
                var data = new TokenApiDto { AccessToken = user.Token, RefreshToken = user.RefreshToken };
                return new ApiResponse(data, true, "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing token.");
                return new ApiResponse(new { error = ex.ToString() });
            }
        }





        public string CreateJwt(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", email)
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var data = await _context.RefreshTokenExists(refreshToken);
            var count = data.Result;
            if (count == 1)
            {
                return await CreateRefreshToken();
            }
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false
            };
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }
    }
}
