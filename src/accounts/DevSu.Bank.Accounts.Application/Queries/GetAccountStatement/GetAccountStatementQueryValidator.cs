using DevSu.Bank.Accounts.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccountStatement
{
    public class GetAccountStatementQueryValidator : AbstractValidator<GetAccountStatementQuery>
    {
        public GetAccountStatementQueryValidator()
        {
            RuleFor(query => query.ClientId)
                .GreaterThanOrEqualTo(1)
                .WithName("cliente")
                .WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"));

            RuleFor(query => query.From)
                .NotNull()
                .WithName("fechaInicial")
                .WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"));

            RuleFor(query => query.To)
                .NotNull()
                .WithName("fechaFinal")
                .WithMessage(string.Format(Generals.NotEmptyOrNullParameter, "{PropertyName}"));

            RuleFor(query => query.To)
                .GreaterThanOrEqualTo(query => query.From)
                .When(query => query.From.HasValue && query.To.HasValue)
                .WithName("fechaFinal")
                .WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "fechaInicial"));
        }
    }
}
