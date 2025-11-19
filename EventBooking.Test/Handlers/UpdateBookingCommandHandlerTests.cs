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
    public class UpdateBookingCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnNull_When_NotFound()
        {
            var mockRepo = new Mock<IBookingRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Booking?)null);

            var handler = new UpdateBookingCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateBookingCommand { Id = Guid.NewGuid(), Update = new UpdateBookingDto { Seats = 3 } }, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Update_When_Found()
        {
            var existing = new Booking { Id = Guid.NewGuid(), Seats = 1 };

            var mockRepo = new Mock<IBookingRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask).Verifiable();

            mockMapper.Setup(m => m.Map(It.IsAny<UpdateBookingDto>(), existing)).Callback<UpdateBookingDto, Booking>((u, b) => { if (u.Seats != null) b.Seats = u.Seats.Value; });
            mockMapper.Setup(m => m.Map<BookingDto>(existing)).Returns(new BookingDto { Id = existing.Id, Seats = existing.Seats });

            var handler = new UpdateBookingCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateBookingCommand { Id = existing.Id, Update = new UpdateBookingDto { Seats = 5 } }, CancellationToken.None);

            result.Should().NotBeNull();
            result.Seats.Should().Be(5);
            mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }
    }
}
