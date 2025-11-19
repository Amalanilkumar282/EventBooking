using FluentValidation;
using EventBooking.Application.Features.Customers.Commands;

namespace EventBooking.Application.Validators
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Customer ID is required");
        }
    }
}
