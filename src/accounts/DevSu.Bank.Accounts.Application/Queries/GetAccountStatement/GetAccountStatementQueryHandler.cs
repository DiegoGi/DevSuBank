using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories.Specifications;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccountStatement
{
    public class GetAccountStatementQueryHandler(
        ITransactionReadOnlyRepository transactionReadOnlyRepository,
        IClientRepository clientRepository)
        : IRequestHandler<GetAccountStatementQuery, IEnumerable<AccountStatementResponse>>
    {
        public async Task<IEnumerable<AccountStatementResponse>> Handle(GetAccountStatementQuery request,
            CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetSingleByIdAsync(request.ClientId)
                ?? throw new NotFoundException(Generals.ClientNotFound);

            var specification = new AccountStatementSpecification(client.Id, request.From!.Value, request.To!.Value);

            var rows = await transactionReadOnlyRepository.ListAsync(specification,
                transaction => new AccountStatementData(
                    transaction.TransactionDate,
                    transaction.Account.Client.Name,
                    transaction.Account.AccountNumber,
                    transaction.Account.AccountType,
                    transaction.Account.InitialBalance,
                    transaction.Account.Status,
                    transaction.Amount,
                    transaction.Balance));

            return rows.Select(AccountStatementResponse.From);
        }
    }
}
