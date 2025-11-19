using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using EventBooking.Application.Features.TicketTypes.Commands;
using EventBooking.Application.Interfaces;

namespace EventBooking.Test.Handlers
{
    public class DeleteTicketTypeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_Delete()
        {
            var mockRepo = new Mock<ITicketTypeRepository>();
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteTicketTypeCommandHandler(mockRepo.Object);

            var result = await handler.Handle(new DeleteTicketTypeCommand { Id = Guid.NewGuid() }, CancellationToken.None);

            result.Should().Be(MediatR.Unit.Value);
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
