using System;
using Xunit;
using FluentAssertions;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.DTOs;
using EventBooking.Application.Validators;

namespace EventBooking.Test.Validators
{
    public class UpdateEventCommandValidatorTests
    {
        private readonly UpdateEventCommandValidator _validator = new UpdateEventCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_IdEmpty()
        {
            var cmd = new UpdateEventCommand { Id = Guid.Empty, Update = new UpdateEventDto { Name = "Name", Capacity = 5, StartDate = DateTime.UtcNow } };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }

        [Fact]
        public void Validate_Should_HaveError_When_UpdateNull()
        {
            var cmd = new UpdateEventCommand { Id = Guid.NewGuid(), Update = null! };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Update");
        }
    }
}
