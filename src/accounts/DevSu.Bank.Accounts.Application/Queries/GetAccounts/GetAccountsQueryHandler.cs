using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories.Specifications;
using DevSu.Bank.Accounts.Application.SeedWork;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccounts
{
    public class GetAccountsQueryHandler(IAccountReadOnlyRepository accountReadOnlyRepository)
        : IRequestHandler<GetAccountsQuery, PagedList<AccountResponse>>
    {
        public async Task<PagedList<AccountResponse>> Handle(GetAccountsQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new AccountSpecification(request.Options);

            var total = await accountReadOnlyRepository.CountAsync(specification);
            var accounts = await accountReadOnlyRepository.ListAsync(specification, AccountResponse.Projection);

            return new PagedList<AccountResponse>(total, accounts);
        }
    }
}
