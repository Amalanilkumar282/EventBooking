using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using EventBooking.Application.Features.TicketTypes.Commands;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Application.DTOs;

namespace EventBooking.Test.Handlers
{
    public class CreateTicketTypeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_CreateTicketType_When_EventExists()
        {
            var dto = new CreateTicketTypeDto { EventId = Guid.NewGuid(), Name = "VIP", Price = 100m, Quantity = 10 };

            var mockRepo = new Mock<ITicketTypeRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockEventRepo.Setup(r => r.ExistsAsync(dto.EventId)).ReturnsAsync(true);

            mockMapper.Setup(m => m.Map<TicketType>(dto)).Returns(new TicketType { EventId = dto.EventId, Name = dto.Name, Price = dto.Price, Quantity = dto.Quantity });
            mockMapper.Setup(m => m.Map<TicketTypeDto>(It.IsAny<TicketType>())).Returns((TicketType t) => new TicketTypeDto { Id = t.Id, EventId = t.EventId, Name = t.Name, Price = t.Price, Quantity = t.Quantity });

            mockRepo.Setup(r => r.AddAsync(It.IsAny<TicketType>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateTicketTypeCommandHandler(mockRepo.Object, mockEventRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateTicketTypeCommand { Create = dto }, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<TicketType>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EventNotExists()
        {
            var dto = new CreateTicketTypeDto { EventId = Guid.NewGuid(), Name = "VIP", Price = 100m, Quantity = 10 };

            var mockRepo = new Mock<ITicketTypeRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockEventRepo.Setup(r => r.ExistsAsync(dto.EventId)).ReturnsAsync(false);

            var handler = new CreateTicketTypeCommandHandler(mockRepo.Object, mockEventRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateTicketTypeCommand { Create = dto }, CancellationToken.None));
        }
    }
}
