using System;
using System.Threading.Tasks;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using Microsoft.Extensions.Configuration;

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

        public AuthService(
            ICustomerRepository customerRepo, 
            ITokenService tokenService, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _customerRepo = customerRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            // Find customer by email
            var customer = await _customerRepo.GetByEmailAsync(loginDto.Email);
            
            if (customer == null)
            {
                // Don't reveal whether email exists for security reasons
                return null;
            }

            // Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.PasswordHash);
            
            if (!isPasswordValid)
            {
                return null;
            }

            // Generate JWT token
            var token = _tokenService.GenerateAccessToken(customer.Id.ToString(), customer.Email);
            
            // Get expiry time from configuration
            var expiryMinutes = int.Parse(_configuration.GetSection("JwtSettings")["ExpiryMinutes"] ?? "60");

            return new LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiryMinutes * 60, // Convert to seconds
                Customer = _mapper.Map<CustomerDto>(customer)
            };
        }

        public async Task<CustomerDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _customerRepo.EmailExistsAsync(registerDto.Email))
            {
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
            
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
