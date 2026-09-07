using DevSu.Bank.Accounts.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Accounts.Application.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(command => command.AccountNumber)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MaximumLength(20).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("numeroCuenta");

            RuleFor(command => command.AccountType)
                .IsInEnum().WithMessage(string.Format(Generals.InvalidValueForParameter, "{PropertyName}"))
                .WithName("tipoCuenta");

            RuleFor(command => command.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("saldoInicial");

            RuleFor(command => command.ClientId)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("clienteId");
        }
    }
}
