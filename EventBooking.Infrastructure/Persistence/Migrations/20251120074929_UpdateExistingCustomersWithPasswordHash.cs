using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventBooking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingCustomersWithPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update all existing customers with the default password hash
            // Password: "Password123"
            // BCrypt hash: $2a$12$iklihucJjkhCg/IQeZ6Wr.GG5J8lCoGZywmBwZL9PKqzht6hZ6QIi
            migrationBuilder.Sql(
                @"UPDATE ""Customers"" 
                  SET ""PasswordHash"" = '$2a$12$iklihucJjkhCg/IQeZ6Wr.GG5J8lCoGZywmBwZL9PKqzht6hZ6QIi' 
                  WHERE ""PasswordHash"" = '' OR ""PasswordHash"" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No need to revert password hashes
        }
    }
}
