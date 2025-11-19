using FluentValidation;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Validators
{
    public class CreateTicketTypeDtoValidator : AbstractValidator<CreateTicketTypeDto>
    {
        public CreateTicketTypeDtoValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("Event ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ticket type name is required")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");
        }
    }
}
