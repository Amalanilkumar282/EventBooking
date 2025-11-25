using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventBooking.Application.Interfaces;

namespace EventBooking.Api.Controllers
{
    /// <summary>
    /// Test controller for debugging authentication issues
    /// DELETE THIS FILE AFTER TESTING!
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : BaseController
    {
        private readonly ICustomerRepository _customerRepo;

        public TestController(ICustomerRepository customerRepo) : base()
        {
            _customerRepo = customerRepo;
        }

        /// <summary>
        /// Test endpoint to check if API is running (no auth required)
        /// </summary>
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok(new { 
                message = "API is running!", 
                timestamp = DateTime.UtcNow,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }

        /// <summary>
        /// Test endpoint to check database connection and customer password hashes
        /// </summary>
        [HttpGet("check-passwords")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPasswords()
        {
            try
            {
                var customers = await _customerRepo.GetAllAsync();
                
                var result = customers.Select(c => new
                {
                    Email = c.Email,
                    FirstName = c.FirstName,
                    HasPasswordHash = !string.IsNullOrEmpty(c.PasswordHash),
                    PasswordHashLength = c.PasswordHash?.Length ?? 0,
                    PasswordHashPrefix = c.PasswordHash?.Length > 10 ? c.PasswordHash.Substring(0, 10) : "EMPTY"
                }).ToList();

                return Ok(new
                {
                    TotalCustomers = customers.Count,
                    CustomersWithPassword = customers.Count(c => !string.IsNullOrEmpty(c.PasswordHash)),
                    CustomersWithoutPassword = customers.Count(c => string.IsNullOrEmpty(c.PasswordHash)),
                    Details = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to check passwords",
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// Test password verification with BCrypt
        /// </summary>
        [HttpPost("test-bcrypt")]
        [AllowAnonymous]
        public IActionResult TestBCrypt([FromBody] TestPasswordRequest request)
        {
            try
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, request.Hash);
                
                return Ok(new
                {
                    password = request.Password,
                    hashPrefix = request.Hash.Length > 20 ? request.Hash.Substring(0, 20) : request.Hash,
                    isValid = isValid,
                    message = isValid ? "Password matches hash!" : "Password does NOT match hash!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "BCrypt verification failed",
                    message = ex.Message
                });
            }
        }
    }

    public class TestPasswordRequest
    {
        public string Password { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
    }
}
