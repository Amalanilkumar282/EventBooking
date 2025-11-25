using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Event persistence operations
    /// </summary>
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        // Expose IQueryable for advanced scenarios (filtering/paging/sorting) when needed
        IQueryable<Event> GetQueryable();

        // Provide paged retrieval to avoid EF Core dependency in Application layer
        Task<List<Event>> GetPagedAsync(string? search, int page, int pageSize);

        // CancellationToken-aware overloads
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken);
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Event ev, CancellationToken cancellationToken);
        Task UpdateAsync(Event ev, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Event>> GetPagedAsync(string? search, int page, int pageSize, CancellationToken cancellationToken);
    }
}
