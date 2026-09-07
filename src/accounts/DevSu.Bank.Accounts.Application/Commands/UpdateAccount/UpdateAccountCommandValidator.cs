using DevSu.Bank.Accounts.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Accounts.Application.Commands.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("id");

            RuleFor(command => command.AccountType)
                .IsInEnum().WithMessage(string.Format(Generals.InvalidValueForParameter, "{PropertyName}"))
                .WithName("tipoCuenta");
        }
    }
}
