using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Application.DTOs;

namespace EventBooking.Test.Handlers
{
    public class UpdateEventCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnNull_When_EventNotFound()
        {
            var mockRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event?)null);

            var handler = new UpdateEventCommandHandler(mockRepo.Object, mockMapper.Object);
            var cmd = new UpdateEventCommand { Id = Guid.NewGuid(), Update = new UpdateEventDto { Name = "Updated" } };

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_UpdateEvent_When_Found()
        {
            var existing = new Event
            {
                Id = Guid.NewGuid(),
                Name = "Old",
                Capacity = 10,
                IsActive = true,
                StartDate = DateTime.UtcNow
            };

            var mockRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask).Verifiable();

            mockMapper.Setup(m => m.Map(It.IsAny<UpdateEventDto>(), existing)).Callback<UpdateEventDto, Event>((u, e) =>
            {
                if (u.Name != null) e.Name = u.Name;
                if (u.Capacity.HasValue) e.Capacity = u.Capacity.Value;
                if (u.IsActive.HasValue) e.IsActive = u.IsActive.Value;
            });

            mockMapper.Setup(m => m.Map<EventDto>(existing)).Returns(new EventDto
            {
                Id = existing.Id,
                Name = existing.Name,
                Capacity = existing.Capacity,
                StartDate = existing.StartDate,
                IsActive = existing.IsActive,
                CreatedAt = existing.CreatedAt
            });

            var handler = new UpdateEventCommandHandler(mockRepo.Object, mockMapper.Object);
            var cmd = new UpdateEventCommand { Id = existing.Id, Update = new UpdateEventDto { Name = "NewName" } };

            var result = await handler.Handle(cmd, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be("NewName");
            mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }
    }
}
