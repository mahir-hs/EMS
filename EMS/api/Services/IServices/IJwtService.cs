using api.Dto.Identity;
using api.Models;
using System.Security.Claims;

namespace api.Services.IServices
{
    public interface IJwtService
    {
        Task<ApiResponse> RefreshToken(TokenApiDto tokenApiDto);

        public string CreateJwt(string email, string role);
        public Task<string> CreateRefreshToken();
        public ClaimsPrincipal GetPrincipleFromExpiredToken(string token);

    }
}
