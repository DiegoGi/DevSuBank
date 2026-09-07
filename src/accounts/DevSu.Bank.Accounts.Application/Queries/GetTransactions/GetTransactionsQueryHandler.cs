using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories.Specifications;
using DevSu.Bank.Accounts.Application.SeedWork;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetTransactions
{
    public class GetTransactionsQueryHandler(ITransactionReadOnlyRepository transactionReadOnlyRepository)
        : IRequestHandler<GetTransactionsQuery, PagedList<TransactionResponse>>
    {
        public async Task<PagedList<TransactionResponse>> Handle(GetTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new TransactionSpecification(request.Options, request.AccountNumber, request.From,
                request.To);

            var total = await transactionReadOnlyRepository.CountAsync(specification);
            var transactions = await transactionReadOnlyRepository.ListAsync(specification,
                TransactionResponse.Projection);

            return new PagedList<TransactionResponse>(total, transactions);
        }
    }
}
