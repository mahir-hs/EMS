using api.Dto.Identity;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api.Services
{
    public class JwtService(IAccountRepository context, IConfiguration config, ILogger<AccountService> logger): IJwtService
    {
        private readonly IConfiguration _config = config;
        private readonly IAccountRepository _context = context;
        private readonly ILogger<AccountService> _logger = logger;

        public async Task<ApiResponse> RefreshToken(TokenApiDto tokenApiDto)
        {
            try
            {
                var principal = GetPrincipleFromExpiredToken(tokenApiDto.AccessToken);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");
                var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == "Role");
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
                user.Token = CreateJwt(emailClaim!.Value, roleClaim!.Value);
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



        public string CreateJwt(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim("Role", role),
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
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
