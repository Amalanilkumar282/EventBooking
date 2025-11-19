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
    public class UpdateTicketTypeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnNull_When_NotFound()
        {
            var mockRepo = new Mock<ITicketTypeRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TicketType?)null);

            var handler = new UpdateTicketTypeCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateTicketTypeCommand { Id = Guid.NewGuid(), Update = new UpdateTicketTypeDto { Price = 10m } }, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Update_When_Found()
        {
            var existing = new TicketType { Id = Guid.NewGuid(), Price = 20m, Quantity = 5, IsActive = true };

            var mockRepo = new Mock<ITicketTypeRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask).Verifiable();

            mockMapper.Setup(m => m.Map(It.IsAny<UpdateTicketTypeDto>(), existing)).Callback<UpdateTicketTypeDto, TicketType>((u, t) => { if (u.Price.HasValue) t.Price = u.Price.Value; if (u.Quantity.HasValue) t.Quantity = u.Quantity.Value; if (u.IsActive.HasValue) t.IsActive = u.IsActive.Value; });
            mockMapper.Setup(m => m.Map<TicketTypeDto>(existing)).Returns(new TicketTypeDto { Id = existing.Id, Price = existing.Price, Quantity = existing.Quantity, IsActive = existing.IsActive });

            var handler = new UpdateTicketTypeCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateTicketTypeCommand { Id = existing.Id, Update = new UpdateTicketTypeDto { Price = 30m, Quantity = 7 } }, CancellationToken.None);

            result.Should().NotBeNull();
            result.Price.Should().Be(30m);
            result.Quantity.Should().Be(7);
            mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }
    }
}
