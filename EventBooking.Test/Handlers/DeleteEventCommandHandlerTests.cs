using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.Interfaces;

namespace EventBooking.Test.Handlers
{
    public class DeleteEventCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_DeleteAsync()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new DeleteEventCommandHandler(mockRepo.Object);
            var cmd = new DeleteEventCommand { Id = Guid.NewGuid() };

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.Should().NotBeNull();
            mockRepo.Verify(r => r.DeleteAsync(cmd.Id), Times.Once);
        }
    }
}
