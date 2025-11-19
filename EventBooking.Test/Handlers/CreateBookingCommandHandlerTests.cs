using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using EventBooking.Application.Features.Bookings.Commands;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Application.DTOs;

namespace EventBooking.Test.Handlers
{
    public class CreateBookingCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_CreateBooking_When_ValidRequest_WithTicketType()
        {
            var dto = new CreateBookingDto
            {
                EventId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                TicketTypeId = Guid.NewGuid(),
                Seats = 2
            };

            var ticketType = new TicketType { Id = dto.TicketTypeId.Value, EventId = dto.EventId, Price = 50m };

            var mockRepo = new Mock<IBookingRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var mockTicketRepo = new Mock<ITicketTypeRepository>();
            var mockMapper = new Mock<IMapper>();

            mockEventRepo.Setup(r => r.ExistsAsync(dto.EventId)).ReturnsAsync(true);
            mockCustomerRepo.Setup(r => r.ExistsAsync(dto.CustomerId)).ReturnsAsync(true);
            mockTicketRepo.Setup(r => r.GetByIdAsync(dto.TicketTypeId.Value)).ReturnsAsync(ticketType);

            mockMapper.Setup(m => m.Map<Booking>(dto)).Returns(new Booking { EventId = dto.EventId, CustomerId = dto.CustomerId, TicketTypeId = dto.TicketTypeId, Seats = dto.Seats });
            mockMapper.Setup(m => m.Map<BookingDto>(It.IsAny<Booking>())).Returns((Booking b) => new BookingDto { Id = b.Id, EventId = b.EventId, CustomerId = b.CustomerId, Seats = b.Seats, TicketTypeId = b.TicketTypeId, TotalPrice = b.TotalPrice, Status = b.Status.ToString(), CreatedAt = b.CreatedAt });

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateBookingCommandHandler(mockRepo.Object, mockEventRepo.Object, mockCustomerRepo.Object, mockTicketRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateBookingCommand { Create = dto }, CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalPrice.Should().Be(ticketType.Price * dto.Seats);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EventNotExists()
        {
            var dto = new CreateBookingDto { EventId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Seats = 1 };

            var mockRepo = new Mock<IBookingRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var mockTicketRepo = new Mock<ITicketTypeRepository>();
            var mockMapper = new Mock<IMapper>();

            mockEventRepo.Setup(r => r.ExistsAsync(dto.EventId)).ReturnsAsync(false);

            var handler = new CreateBookingCommandHandler(mockRepo.Object, mockEventRepo.Object, mockCustomerRepo.Object, mockTicketRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateBookingCommand { Create = dto }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_CustomerNotExists()
        {
            var dto = new CreateBookingDto { EventId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Seats = 1 };

            var mockRepo = new Mock<IBookingRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var mockTicketRepo = new Mock<ITicketTypeRepository>();
            var mockMapper = new Mock<IMapper>();

            mockEventRepo.Setup(r => r.ExistsAsync(dto.EventId)).ReturnsAsync(true);
            mockCustomerRepo.Setup(r => r.ExistsAsync(dto.CustomerId)).ReturnsAsync(false);

            var handler = new CreateBookingCommandHandler(mockRepo.Object, mockEventRepo.Object, mockCustomerRepo.Object, mockTicketRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateBookingCommand { Create = dto }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_TicketTypeDoesNotBelongToEvent()
        {
            var dto = new CreateBookingDto { EventId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), TicketTypeId = Guid.NewGuid(), Seats = 1 };

            var mockRepo = new Mock<IBookingRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var mockTicketRepo = new Mock<ITicketTypeRepository>();
            var mockMapper = new Mock<IMapper>();

            mockEventRepo.Setup(r => r.ExistsAsync(dto.EventId)).ReturnsAsync(true);
            mockCustomerRepo.Setup(r => r.ExistsAsync(dto.CustomerId)).ReturnsAsync(true);

            // ticketType belongs to different event
            mockTicketRepo.Setup(r => r.GetByIdAsync(dto.TicketTypeId.Value)).ReturnsAsync(new TicketType { Id = dto.TicketTypeId.Value, EventId = Guid.NewGuid(), Price = 10m });

            var handler = new CreateBookingCommandHandler(mockRepo.Object, mockEventRepo.Object, mockCustomerRepo.Object, mockTicketRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateBookingCommand { Create = dto }, CancellationToken.None));
        }
    }
}
