using System;
using System.Collections.Generic;
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
    }
}
