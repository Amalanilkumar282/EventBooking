namespace EventBooking.Application.Interfaces
{
    /// <summary>
    /// Service for generating JWT tokens
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT access token for a customer
        /// </summary>
        /// <param name="customerId">Customer's unique ID</param>
        /// <param name="email">Customer's email</param>
        /// <returns>JWT token string</returns>
        string GenerateAccessToken(string customerId, string email);
    }
}
