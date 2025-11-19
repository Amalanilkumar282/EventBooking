using System;
using Xunit;
using FluentAssertions;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.DTOs;
using EventBooking.Application.Validators;

namespace EventBooking.Test.Validators
{
    public class CreateEventCommandValidatorTests
    {
        private readonly CreateEventCommandValidator _validator = new CreateEventCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_CreateIsNull()
        {
            var cmd = new CreateEventCommand { Create = null! };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Create");
        }

        [Fact]
        public void Validate_Should_Pass_When_ValidCreateDto()
        {
            var dto = new CreateEventDto
            {
                Name = "Test Event",
                Capacity = 10,
                StartDate = DateTime.UtcNow.AddDays(1)
            };

            var cmd = new CreateEventCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeTrue();
        }
    }
}
