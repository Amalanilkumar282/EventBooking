using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Domain.Entities;
using System.Collections.Generic;

namespace EventBooking.Infrastructure.Persistence
{
    /// <summary>
    /// Simple data seeder to insert initial rows for development and manual testing.
    /// It is idempotent: it returns immediately if events already exist.
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(EventBookingDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // if database already has data, do nothing
            if (await context.Events.AnyAsync()) return;

            // Prepare events
            var events = new List<Event>
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Rock Fest 2026",
                    Description = "Annual open-air rock music festival featuring international and local bands.",
                    Venue = "Riverside Park",
                    StartDate = DateTime.UtcNow.AddMonths(2),
                    EndDate = DateTime.UtcNow.AddMonths(2).AddDays(1),
                    Capacity = 5000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Tech Conference 2026",
                    Description = "Three-day technology conference with keynotes, workshops and networking.",
                    Venue = "Convention Center",
                    StartDate = DateTime.UtcNow.AddMonths(1),
                    EndDate = DateTime.UtcNow.AddMonths(1).AddDays(2),
                    Capacity = 1200,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Name = "Food Carnival 2026",
                    Description = "A weekend food carnival with food trucks, cooking demos and tasting sessions.",
                    Venue = "City Square",
                    StartDate = DateTime.UtcNow.AddMonths(3),
                    EndDate = DateTime.UtcNow.AddMonths(3).AddDays(1),
                    Capacity = 3000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            // Prepare ticket types for each event
            var ticketTypes = new List<TicketType>();

            // Rock Fest ticket types
            var rockVip = new TicketType { Id = Guid.NewGuid(), EventId = events[0].Id, Name = "VIP", Description = "Access to VIP area + free drinks", Price = 250.00m, Quantity = 200, Sold = 0, IsActive = true };
            var rockGeneral = new TicketType { Id = Guid.NewGuid(), EventId = events[0].Id, Name = "General", Description = "General admission", Price = 60.00m, Quantity = 4800, Sold = 0, IsActive = true };
            var rockEarly = new TicketType { Id = Guid.NewGuid(), EventId = events[0].Id, Name = "Early Bird", Description = "Discounted early tickets", Price = 40.00m, Quantity = 500, Sold = 0, IsActive = true };
            ticketTypes.AddRange(new[] { rockVip, rockGeneral, rockEarly });

            // Tech Conference ticket types
            var techFull = new TicketType { Id = Guid.NewGuid(), EventId = events[1].Id, Name = "Full Pass", Description = "Access to all days and workshops", Price = 499.00m, Quantity = 800, Sold = 0, IsActive = true };
            var techExpo = new TicketType { Id = Guid.NewGuid(), EventId = events[1].Id, Name = "Expo Only", Description = "Expo hall access and exhibitors", Price = 99.00m, Quantity = 1000, Sold = 0, IsActive = true };
            ticketTypes.AddRange(new[] { techFull, techExpo });

            // Food Carnival ticket types
            var foodEntry = new TicketType { Id = Guid.NewGuid(), EventId = events[2].Id, Name = "Entry", Description = "General entry to carnival grounds", Price = 10.00m, Quantity = 3000, Sold = 0, IsActive = true };
            var foodTasting = new TicketType { Id = Guid.NewGuid(), EventId = events[2].Id, Name = "Tasting Pass", Description = "Includes 5 tasting vouchers", Price = 25.00m, Quantity = 800, Sold = 0, IsActive = true };
            ticketTypes.AddRange(new[] { foodEntry, foodTasting });

            // Prepare customers
            // Password for all seed customers: "Password123" (hashed using BCrypt)
            var passwordHash = "$2a$12$iklihucJjkhCg/IQeZ6Wr.GG5J8lCoGZywmBwZL9PKqzht6hZ6QIi";
            
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", PhoneNumber = "555-0001", PasswordHash = passwordHash, CreatedAt = DateTime.UtcNow },
                new Customer { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", PhoneNumber = "555-0002", PasswordHash = passwordHash, CreatedAt = DateTime.UtcNow },
                new Customer { Id = Guid.NewGuid(), FirstName = "Carol", LastName = "Williams", Email = "carol.williams@example.com", PhoneNumber = "555-0003", PasswordHash = passwordHash, CreatedAt = DateTime.UtcNow },
                new Customer { Id = Guid.NewGuid(), FirstName = "David", LastName = "Brown", Email = "david.brown@example.com", PhoneNumber = "555-0004", PasswordHash = passwordHash, CreatedAt = DateTime.UtcNow },
                new Customer { Id = Guid.NewGuid(), FirstName = "Eve", LastName = "Davis", Email = "eve.davis@example.com", PhoneNumber = "555-0005", PasswordHash = passwordHash, CreatedAt = DateTime.UtcNow },
                new Customer { Id = Guid.NewGuid(), FirstName = "Frank", LastName = "Miller", Email = "frank.miller@example.com", PhoneNumber = "555-0006", PasswordHash = passwordHash, CreatedAt = DateTime.UtcNow }
            };

            // Prepare bookings across events
            var bookings = new List<Booking>
            {
                new Booking { Id = Guid.NewGuid(), EventId = events[0].Id, Event = events[0], CustomerId = customers[0].Id, Customer = customers[0], TicketTypeId = rockVip.Id, TicketType = rockVip, Seats = 2, TotalPrice = rockVip.Price * 2, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow },
                new Booking { Id = Guid.NewGuid(), EventId = events[0].Id, Event = events[0], CustomerId = customers[1].Id, Customer = customers[1], TicketTypeId = rockGeneral.Id, TicketType = rockGeneral, Seats = 4, TotalPrice = rockGeneral.Price * 4, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow },
                new Booking { Id = Guid.NewGuid(), EventId = events[1].Id, Event = events[1], CustomerId = customers[2].Id, Customer = customers[2], TicketTypeId = techFull.Id, TicketType = techFull, Seats = 1, TotalPrice = techFull.Price * 1, Status = BookingStatus.Pending, CreatedAt = DateTime.UtcNow },
                new Booking { Id = Guid.NewGuid(), EventId = events[1].Id, Event = events[1], CustomerId = customers[3].Id, Customer = customers[3], TicketTypeId = techExpo.Id, TicketType = techExpo, Seats = 3, TotalPrice = techExpo.Price * 3, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow },
                new Booking { Id = Guid.NewGuid(), EventId = events[2].Id, Event = events[2], CustomerId = customers[4].Id, Customer = customers[4], TicketTypeId = foodEntry.Id, TicketType = foodEntry, Seats = 5, TotalPrice = foodEntry.Price * 5, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow },
                new Booking { Id = Guid.NewGuid(), EventId = events[2].Id, Event = events[2], CustomerId = customers[5].Id, Customer = customers[5], TicketTypeId = foodTasting.Id, TicketType = foodTasting, Seats = 2, TotalPrice = foodTasting.Price * 2, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow }
            };

            // Update sold counts for ticket types based on bookings
            void IncrementSold(Guid ticketTypeId, int seats)
            {
                var tt = ticketTypes.Find(t => t.Id == ticketTypeId);
                if (tt != null) tt.Sold += seats;
            }

            foreach (var b in bookings)
            {
                if (b.TicketTypeId.HasValue)
                    IncrementSold(b.TicketTypeId.Value, b.Seats);
            }

            // Add all to context
            await context.Events.AddRangeAsync(events);
            await context.TicketTypes.AddRangeAsync(ticketTypes);
            await context.Customers.AddRangeAsync(customers);
            await context.Bookings.AddRangeAsync(bookings);

            await context.SaveChangesAsync();
        }
    }
}
