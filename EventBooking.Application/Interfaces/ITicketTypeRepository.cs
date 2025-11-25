using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Interfaces
{
    /// <summary>
    /// Repository interface for TicketType persistence operations
    /// </summary>
    public interface ITicketTypeRepository
    {
        Task<List<TicketType>> GetAllAsync();
        Task<TicketType?> GetByIdAsync(Guid id);
        Task<List<TicketType>> GetByEventIdAsync(Guid eventId);
        Task AddAsync(TicketType ticketType);
        Task UpdateAsync(TicketType ticketType);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        // CancellationToken-aware overloads
        Task<List<TicketType>> GetAllAsync(CancellationToken cancellationToken);
        Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<TicketType>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
        Task AddAsync(TicketType ticketType, CancellationToken cancellationToken);
        Task UpdateAsync(TicketType ticketType, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}
