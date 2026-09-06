using DevSu.Bank.Customers.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Customers.Application.Commands.ChangeClientPassword
{
    public class ChangeClientPasswordCommandValidator : AbstractValidator<ChangeClientPasswordCommand>
    {
        public ChangeClientPasswordCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"));

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MinimumLength(4).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{MinLength}"));
        }
    }
}
