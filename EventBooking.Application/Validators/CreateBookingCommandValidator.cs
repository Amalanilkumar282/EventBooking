using FluentValidation;
using EventBooking.Application.Features.Bookings.Commands;

namespace EventBooking.Application.Validators
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.Create).NotNull().WithMessage("Create payload is required");
            RuleFor(x => x.Create.EventId).NotEmpty().WithMessage("EventId is required");
            RuleFor(x => x.Create.CustomerId).NotEmpty().WithMessage("CustomerId is required");
            RuleFor(x => x.Create.Seats).GreaterThan(0).WithMessage("Seats must be at least 1");
        }
    }
}
