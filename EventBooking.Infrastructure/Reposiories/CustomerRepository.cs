using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        // Parameterless methods for compatibility
        public Task<List<Customer>> GetAllAsync() => GetAllAsync(CancellationToken.None);
        public Task<Customer?> GetByIdAsync(Guid id) => GetByIdAsync(id, CancellationToken.None);
        public Task<Customer?> GetByEmailAsync(string email) => GetByEmailAsync(email, CancellationToken.None);
        public Task AddAsync(Customer customer) => AddAsync(customer, CancellationToken.None);
        public Task UpdateAsync(Customer customer) => UpdateAsync(customer, CancellationToken.None);
        public Task DeleteAsync(Guid id) => DeleteAsync(id, CancellationToken.None);
        public Task<bool> ExistsAsync(Guid id) => ExistsAsync(id, CancellationToken.None);
        public Task<bool> EmailExistsAsync(string email, Guid? excludeId = null) => EmailExistsAsync(email, excludeId, CancellationToken.None);
        public Task<List<Customer>> GetPagedAsync(int page, int pageSize) => GetPagedAsync(page, pageSize, CancellationToken.None);

        public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching all customers");
            return await _db.Customers.ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching customer by Id={CustomerId}", id);
            return await _db.Customers.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching customer by Email={Email}", email);
            return await _db.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Adding customer Email={Email}", customer.Email);
            await _db.Customers.AddAsync(customer, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Customer added CustomerId={CustomerId}, Email={Email}", customer.Id, customer.Email);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Updating customer CustomerId={CustomerId}", customer.Id);
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Customer updated CustomerId={CustomerId}", customer.Id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Deleting customer CustomerId={CustomerId}", id);
            var customer = await _db.Customers.FindAsync(new object[] { id }, cancellationToken);
            if (customer == null) return;
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Customer deleted CustomerId={CustomerId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Checking existence for CustomerId={CustomerId}", id);
            return await _db.Customers.AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Checking if email exists Email={Email} ExcludeId={ExcludeId}", email, excludeId);
            if (excludeId.HasValue)
            {
                return await _db.Customers.AnyAsync(c => c.Email == email && c.Id != excludeId.Value, cancellationToken);
            }
            return await _db.Customers.AnyAsync(c => c.Email == email, cancellationToken);
        }

        public IQueryable<Customer> GetQueryable()
        {
            // Return IQueryable so callers can compose additional filters/pagination.
            // Important: do not call ToList() here; let caller decide when to execute the query.
            return _db.Customers.AsQueryable();
        }

        public async Task<List<Customer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var ps = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);
            var p = page <= 0 ? 1 : page;

            return await _db.Customers
                .OrderBy(c => c.CreatedAt)
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync(cancellationToken);
        }
    }
}
