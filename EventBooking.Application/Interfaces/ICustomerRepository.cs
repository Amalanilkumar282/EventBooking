using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Customer persistence operations
    /// </summary>
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetByEmailAsync(string email);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);

        // New overloads accepting CancellationToken
        Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken);
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Customer customer, CancellationToken cancellationToken);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> EmailExistsAsync(string email, Guid? excludeId, CancellationToken cancellationToken);

        // Expose an IQueryable for advanced scenarios (filtering/paging) when needed.
        // Use with care: this exposes EF Core queryable and keeps data access details.
        IQueryable<Customer> GetQueryable();

        // Provide an async paged query to keep paging implementation inside infrastructure
        Task<List<Customer>> GetPagedAsync(int page, int pageSize);
        Task<List<Customer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    }
}
