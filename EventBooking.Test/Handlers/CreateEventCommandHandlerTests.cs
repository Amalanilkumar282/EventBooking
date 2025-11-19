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
    public class CreateEventCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_CreateEvent_When_ValidRequest()
        {
            // Arrange
            var dto = new CreateEventDto
            {
                Name = "New Event",
                Capacity = 50,
                StartDate = DateTime.UtcNow.AddDays(1)
            };

            var mockRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<Event>(dto)).Returns(new Event
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                StartDate = dto.StartDate
            });

            mockMapper.Setup(m => m.Map<EventDto>(It.IsAny<Event>())).Returns((Event e) => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Capacity = e.Capacity,
                StartDate = e.StartDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            });

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Event>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateEventCommandHandler(mockRepo.Object, mockMapper.Object);
            var cmd = new CreateEventCommand { Create = dto };

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
        }
    }
}
