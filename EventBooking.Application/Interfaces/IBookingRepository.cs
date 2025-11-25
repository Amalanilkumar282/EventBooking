using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Booking persistence operations
    /// </summary>
    public interface IBookingRepository
    {
        // Original parameterless signatures retained for mocks/tests
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(Guid id);
        Task<List<Booking>> GetByCustomerIdAsync(Guid customerId);
        Task<List<Booking>> GetByEventIdAsync(Guid eventId);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        // New overloads that accept CancellationToken for cooperative cancellation
        Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken);
        Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<List<Booking>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
        Task AddAsync(Booking booking, CancellationToken cancellationToken);
        Task UpdateAsync(Booking booking, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

        // Expose an IQueryable for advanced queries (filtering, paging)
        IQueryable<Booking> GetQueryable();

        // Provide paged retrieval to avoid EF Core dependency in Application layer
        Task<List<Booking>> GetPagedAsync(int page, int pageSize);
        Task<List<Booking>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    }
}
