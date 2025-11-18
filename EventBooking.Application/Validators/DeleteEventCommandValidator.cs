using FluentValidation;
using EventBooking.Application.Features.Events.Commands;

namespace EventBooking.Application.Validators
{
    public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
    {
        public DeleteEventCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Event Id is required");
        }
    }
}
