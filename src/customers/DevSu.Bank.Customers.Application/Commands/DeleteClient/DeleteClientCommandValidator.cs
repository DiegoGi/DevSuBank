using DevSu.Bank.Customers.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Customers.Application.Commands.DeleteClient
{
    public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
    {
        public DeleteClientCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("id");
        }
    }
}
