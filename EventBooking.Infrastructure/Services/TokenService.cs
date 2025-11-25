using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventBooking.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace EventBooking.Infrastructure.Services
{
    /// <summary>
    /// Service for generating JWT tokens
    /// This uses the JWT settings from appsettings.json
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generates the access token.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns>A string representing the generated JWT access token.</returns>
        public string GenerateAccessToken(string customerId, string email)
        {
            _logger.LogDebug("Generating access token for CustomerId={CustomerId}, Email={Email}", customerId, email);

            // Read JWT settings from configuration
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
            var issuer = jwtSettings["Issuer"] ?? "EventBookingApi";
            var audience = jwtSettings["Audience"] ?? "EventBookingClient";
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            // Create claims - these are pieces of information stored in the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customerId), // Subject (user ID)
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim(ClaimTypes.NameIdentifier, customerId)
            };

            // Create signing credentials using the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogDebug("Access token generated for CustomerId={CustomerId} with expiryMinutes={ExpiryMinutes}", customerId, expiryMinutes);

            // Convert token to string
            return tokenString;
        }
    }
}
