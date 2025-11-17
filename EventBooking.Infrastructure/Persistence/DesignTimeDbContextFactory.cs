using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventBooking.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EventBookingDbContext>
    {
        public EventBookingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EventBookingDbContext>();
            // use the verified local postgres credentials
            var conn = "Host=localhost;Port=5432;Database=EventBookingDb;Username=postgres;Password=experion@123";
            builder.UseNpgsql(conn, b => b.MigrationsAssembly("EventBooking.Infrastructure"));
            return new EventBookingDbContext(builder.Options);
        }
    }
}
