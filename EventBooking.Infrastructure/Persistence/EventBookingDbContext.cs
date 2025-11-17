using System;
using Microsoft.EntityFrameworkCore;
using EventBooking.Domain.Entities;

namespace EventBooking.Infrastructure.Persistence
{
    public class EventBookingDbContext : DbContext
    {
        public EventBookingDbContext(DbContextOptions<EventBookingDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<TicketType> TicketTypes { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Event
            modelBuilder.Entity<Event>(eb =>
            {
                eb.HasKey(e => e.Id);
                eb.Property(e => e.Name).IsRequired().HasMaxLength(200);
                eb.Property(e => e.Venue).HasMaxLength(200);
                eb.Property(e => e.Description).HasMaxLength(1000);
                eb.Property(e => e.CreatedAt).IsRequired();

                eb.HasMany(e => e.TicketTypes)
                  .WithOne(t => t.Event)
                  .HasForeignKey(t => t.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.HasMany(e => e.Bookings)
                  .WithOne(b => b.Event)
                  .HasForeignKey(b => b.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // TicketType
            modelBuilder.Entity<TicketType>(tb =>
            {
                tb.HasKey(t => t.Id);
                tb.Property(t => t.Name).IsRequired().HasMaxLength(150);
                tb.Property(t => t.Description).HasMaxLength(500);
                tb.Property(t => t.Price).HasPrecision(18, 2);
                tb.Property(t => t.Quantity).IsRequired();
                tb.Property(t => t.Sold).IsRequired();
            });

            // Customer
            modelBuilder.Entity<Customer>(cb =>
            {
                cb.HasKey(c => c.Id);
                cb.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
                cb.Property(c => c.LastName).IsRequired().HasMaxLength(100);
                cb.Property(c => c.Email).IsRequired().HasMaxLength(256);
                cb.HasIndex(c => c.Email).IsUnique();
                cb.Property(c => c.CreatedAt).IsRequired();

                cb.HasMany(c => c.Bookings)
                  .WithOne(b => b.Customer)
                  .HasForeignKey(b => b.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // Booking
            modelBuilder.Entity<Booking>(bb =>
            {
                bb.HasKey(b => b.Id);
                bb.Property(b => b.Seats).IsRequired();
                bb.Property(b => b.TotalPrice).HasPrecision(18, 2);
                bb.Property(b => b.Status).IsRequired();
                bb.Property(b => b.CreatedAt).IsRequired();

                // Concurrency token
                bb.Property(b => b.RowVersion)
                  .IsRowVersion();

                bb.HasOne(b => b.TicketType)
                  .WithMany()
                  .HasForeignKey(b => b.TicketTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
