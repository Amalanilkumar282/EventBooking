using FluentValidation;
using EventBooking.Application.Features.Bookings.Commands;

namespace EventBooking.Application.Validators
{
    public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
    {
        public UpdateBookingCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Booking ID is required");
            RuleFor(x => x.Update).NotNull().WithMessage("Update payload is required");

            RuleFor(x => x.Update.Seats)
                .GreaterThan(0).WithMessage("Seats must be greater than zero")
                .When(x => x.Update != null && x.Update.Seats.HasValue);

            RuleFor(x => x.Update.Status)
                .Must(status => status == "Pending" || status == "Confirmed" || status == "Cancelled")
                .WithMessage("Status must be either 'Pending', 'Confirmed', or 'Cancelled'")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.Status));
        }
    }
}
