using DevSu.Bank.Accounts.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Accounts.Application.Queries.GetTransactions
{
    public class GetTransactionsQueryValidator : AbstractValidator<GetTransactionsQuery>
    {
        public GetTransactionsQueryValidator()
        {
            RuleFor(query => query.Options.Page)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"));

            RuleFor(query => query.Options.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .LessThanOrEqualTo(100).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{ComparisonValue}"));

            RuleFor(query => query.To)
                .GreaterThanOrEqualTo(query => query.From)
                .When(query => query.From.HasValue && query.To.HasValue)
                .WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"));
        }
    }
}
