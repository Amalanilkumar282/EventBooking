using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace EventBooking.Infrastructure.Reposiories
{
    /// <summary>
    /// Entity Framework implementation of ICustomerRepository
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EventBookingDbContext _db;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(EventBookingDbContext db, ILogger<CustomerRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all customers");
            return await _db.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Fetching customer by Id={CustomerId}", id);
            return await _db.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            _logger.LogDebug("Fetching customer by Email={Email}", email);
            return await _db.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddAsync(Customer customer)
        {
            _logger.LogDebug("Adding customer Email={Email}", customer.Email);
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Customer added CustomerId={CustomerId}, Email={Email}", customer.Id, customer.Email);
        }

        public async Task UpdateAsync(Customer customer)
        {
            _logger.LogDebug("Updating customer CustomerId={CustomerId}", customer.Id);
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Customer updated CustomerId={CustomerId}", customer.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogDebug("Deleting customer CustomerId={CustomerId}", id);
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return;
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Customer deleted CustomerId={CustomerId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            _logger.LogDebug("Checking existence for CustomerId={CustomerId}", id);
            return await _db.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        {
            _logger.LogDebug("Checking if email exists Email={Email} ExcludeId={ExcludeId}", email, excludeId);
            if (excludeId.HasValue)
            {
                return await _db.Customers.AnyAsync(c => c.Email == email && c.Id != excludeId.Value);
            }
            return await _db.Customers.AnyAsync(c => c.Email == email);
        }
    }
}
