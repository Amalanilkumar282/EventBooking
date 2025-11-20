using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Api.Controllers
{
    /// <summary>
    /// Authentication controller for login and registration
    /// These endpoints do NOT require authorization (they're public)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login endpoint - returns JWT token on successful authentication
        /// </summary>
        /// <param name="loginDto">Email and password</param>
        /// <returns>Access token and customer info</returns>
        [HttpPost("login")]
        [AllowAnonymous] // This explicitly allows unauthenticated access
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (result == null)
            {
                return Unauthorized(new 
                { 
                    type = "https://example.com/authentication-error",
                    title = "Invalid credentials",
                    status = 401 
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Register endpoint - creates a new customer account
        /// </summary>
        /// <param name="registerDto">Registration information including password</param>
        /// <returns>Created customer information (without password)</returns>
        [HttpPost("register")]
        [AllowAnonymous] // This explicitly allows unauthenticated access
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var customer = await _authService.RegisterAsync(registerDto);
            return CreatedAtAction(nameof(Login), new { email = customer.Email }, customer);
        }
    }
}
