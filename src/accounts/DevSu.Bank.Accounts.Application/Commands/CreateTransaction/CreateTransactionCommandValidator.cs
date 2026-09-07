using DevSu.Bank.Accounts.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Accounts.Application.Commands.CreateTransaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(command => command.AccountNumber)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MaximumLength(20).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("numeroCuenta");

            RuleFor(command => command.Amount)
                .NotEqual(0).WithMessage(string.Format(Generals.InvalidValueForParameter, "{PropertyName}"))
                .WithName("valor");
        }
    }
}
