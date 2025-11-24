using System;
using System.Threading.Tasks;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventBooking.Infrastructure.Services
{
    /// <summary>
    /// Authentication service that handles login and registration
    /// Uses BCrypt for password hashing
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ICustomerRepository customerRepo, 
            ITokenService tokenService, 
            IMapper mapper,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _customerRepo = customerRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Logs in a customer using the provided credentials
        /// </summary>
        /// <param name="loginDto">The login credentials</param>
        /// <returns>A task that represents the asynchronous operation
        /// The task result contains the login response data transfer object</returns>
        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            _logger.LogDebug("Attempting login for Email={Email}", loginDto.Email);

            // Find customer by email
            var customer = await _customerRepo.GetByEmailAsync(loginDto.Email);
            
            if (customer == null)
            {
                _logger.LogWarning("Login failed for Email={Email}: user not found", loginDto.Email);
                // Don't reveal whether email exists for security reasons
                return null;
            }

            // Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.PasswordHash);
            
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed for Email={Email}: invalid password", loginDto.Email);
                return null;
            }

            // Generate JWT token
            var token = _tokenService.GenerateAccessToken(customer.Id.ToString(), customer.Email);
            
            // Get expiry time from configuration
            var expiryMinutes = int.Parse(_configuration.GetSection("JwtSettings")["ExpiryMinutes"] ?? "60");

            _logger.LogInformation("Login succeeded for CustomerId={CustomerId}, Email={Email}", customer.Id, customer.Email);

            return new LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiryMinutes * 60, // Convert to seconds
                Customer = _mapper.Map<CustomerDto>(customer)
            };
        }

        /// <summary>
        /// Registers a new customer
        /// </summary>
        /// <param name="registerDto">The registration data</param>
        /// <returns>A task that represents the asynchronous operation
        /// The task result contains the registered customer data transfer object</returns>
        public async Task<CustomerDto> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogDebug("Registering new customer Email={Email}", registerDto.Email);

            // Check if email already exists
            if (await _customerRepo.EmailExistsAsync(registerDto.Email))
            {
                _logger.LogWarning("Registration failed for Email={Email}: email already exists", registerDto.Email);
                throw new InvalidOperationException($"Customer with email '{registerDto.Email}' already exists.");
            }

            // Hash the password using BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create customer entity
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _customerRepo.AddAsync(customer);

            _logger.LogInformation("Customer registered CustomerId={CustomerId}, Email={Email}", customer.Id, customer.Email);
            
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
