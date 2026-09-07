using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandler(IAccountReadOnlyRepository accountReadOnlyRepository)
        : IRequestHandler<GetAccountByIdQuery, AccountResponse>
    {
        public async Task<AccountResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await accountReadOnlyRepository.GetSingleAsync(account => account.Id == request.Id,
                AccountResponse.Projection);

            return account ?? throw new NotFoundException(Generals.AccountNotFound);
        }
    }
}
