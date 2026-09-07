using DevSu.Bank.Customers.Domain.Resources;
using FluentValidation;

namespace DevSu.Bank.Customers.Application.Queries.GetClients
{
    public class GetClientsQueryValidator : AbstractValidator<GetClientsQuery>
    {
        public GetClientsQueryValidator()
        {
            RuleFor(query => query.Options.Page)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("pagina");

            RuleFor(query => query.Options.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(Generals.GreaterOrEqualTo, "{PropertyName}", "{ComparisonValue}"))
                .LessThanOrEqualTo(100).WithMessage(string.Format(Generals.LessThanOrEqualThanTo, "{PropertyName}", "{ComparisonValue}"))
                .WithName("tamanoPagina");
        }
    }
}
