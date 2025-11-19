using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using EventBooking.Application.Features.Bookings.Commands;
using EventBooking.Application.Interfaces;

namespace EventBooking.Test.Handlers
{
    public class DeleteBookingCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_Delete()
        {
            var mockRepo = new Mock<IBookingRepository>();
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteBookingCommandHandler(mockRepo.Object);

            var result = await handler.Handle(new DeleteBookingCommand { Id = Guid.NewGuid() }, CancellationToken.None);

            result.Should().Be(MediatR.Unit.Value);
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
