using FluentValidation;
using EventBooking.Application.Features.Bookings.Commands;

namespace EventBooking.Application.Validators
{
    public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
    {
        public DeleteBookingCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Booking ID is required");
        }
    }
}
