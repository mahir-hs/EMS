using api.Dto.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using api.Models;
using api.Dto.Account;
using api.Services.IServices;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService context, ILogger<AccountController> logger) : ControllerBase
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IAccountService _context = context;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to Login");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to Login", "400"));
            }

            try
            {
                var result = await _context.Login(user);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to Login");
                    return BadRequest(result);
                }

                var token = new TokenApiDto()
                {
                    AccessToken = result.Result!.AccessToken,
                    RefreshToken = result.Result.RefreshToken
                };

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(500, new { message = "An error occurred while processing your request. " + ex.Message });
            }


        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] RegisterDto user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to Register");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to Register", "400"));
            }

            try
            {
                var userObj = await _context.Register(user);
                if (!userObj.Success)
                {
                    _logger.LogWarning("Failed to Register");
                    return BadRequest(userObj);
                }

                return Ok(userObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to refresh token");
                return BadRequest(new ApiResponse(ModelState, false, "Failed to refresh token", "400"));
            }

            try
            {
                var response = await _context.RefreshToken(tokenApiDto);
                if (!response.Success)
                {
                    _logger.LogWarning("Failed to refresh token");
                    return BadRequest(response);
                }

                return Ok(new TokenApiDto()
                {
                    AccessToken = response.Result!.AccessToken,
                    RefreshToken = response.Result.RefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string accessToken)
        {
            try
            {
                Console.WriteLine(accessToken);
                var response = await _context.LogOut(accessToken);
                if (!response.Success)
                {
                    _logger.LogWarning("Failed to logout");
                    return BadRequest(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                return StatusCode(500, new { message = "An error occurred while processing your request from controller." });
            }
        }
    }
}
