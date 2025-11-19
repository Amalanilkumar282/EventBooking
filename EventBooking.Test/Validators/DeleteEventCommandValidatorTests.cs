using System;
using Xunit;
using FluentAssertions;
using EventBooking.Application.Features.Events.Commands;
using EventBooking.Application.Validators;

namespace EventBooking.Test.Validators
{
    public class DeleteEventCommandValidatorTests
    {
        private readonly DeleteEventCommandValidator _validator = new DeleteEventCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_IdEmpty()
        {
            var cmd = new DeleteEventCommand { Id = Guid.Empty };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }

        [Fact]
        public void Validate_Should_Pass_When_IdProvided()
        {
            var cmd = new DeleteEventCommand { Id = Guid.NewGuid() };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeTrue();
        }
    }
}
