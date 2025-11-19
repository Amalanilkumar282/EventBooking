using FluentValidation;
using EventBooking.Application.Features.TicketTypes.Commands;

namespace EventBooking.Application.Validators
{
    public class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
    {
        public CreateTicketTypeCommandValidator()
        {
            RuleFor(x => x.Create).NotNull().WithMessage("Create payload is required");

            RuleFor(x => x.Create.EventId)
                .NotEmpty().WithMessage("Event ID is required")
                .When(x => x.Create != null);

            RuleFor(x => x.Create.Name)
                .NotEmpty().WithMessage("Ticket type name is required")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters")
                .When(x => x.Create != null);

            RuleFor(x => x.Create.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => x.Create != null && !string.IsNullOrEmpty(x.Create.Description));

            RuleFor(x => x.Create.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero")
                .When(x => x.Create != null);

            RuleFor(x => x.Create.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero")
                .When(x => x.Create != null);
        }
    }
}
