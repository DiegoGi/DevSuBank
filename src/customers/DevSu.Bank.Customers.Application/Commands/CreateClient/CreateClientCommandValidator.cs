using DevSu.Bank.Customers.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Customers.Application.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MaximumLength(150).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("nombre");

            RuleFor(command => command.Gender)
                .IsInEnum().WithMessage(string.Format(Generals.InvalidValueForParameter, "{PropertyName}"))
                .WithName("genero");

            RuleFor(command => command.Age)
                .GreaterThanOrEqualTo(0).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .LessThanOrEqualTo(120).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("edad");

            RuleFor(command => command.Identification)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MaximumLength(30).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("identificacion");

            RuleFor(command => command.Address)
                .MaximumLength(250).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("direccion");

            RuleFor(command => command.Phone)
                .MaximumLength(30).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("telefono");

            RuleFor(command => command.ClientId)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MaximumLength(20).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{MaxLength}"))
                .WithName("clienteid");

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"))
                .MinimumLength(4).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{MinLength}"))
                .WithName("contrasena");
        }
    }
}
