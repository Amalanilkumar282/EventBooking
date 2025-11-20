using System.Threading.Tasks;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Interfaces
{
    /// <summary>
    /// Service for handling authentication operations (login, token generation)
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a customer and returns a JWT token
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>Login response with access token</returns>
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Registers a new customer with hashed password
        /// </summary>
        /// <param name="registerDto">Registration information</param>
        /// <returns>Customer DTO of created customer</returns>
        Task<CustomerDto> RegisterAsync(RegisterDto registerDto);
    }
}
