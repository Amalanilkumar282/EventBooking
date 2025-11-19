using FluentValidation;
using EventBooking.Application.Features.TicketTypes.Commands;

namespace EventBooking.Application.Validators
{
    public class DeleteTicketTypeCommandValidator : AbstractValidator<DeleteTicketTypeCommand>
    {
        public DeleteTicketTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("TicketType ID is required");
        }
    }
}
