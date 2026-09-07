using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandler(ITransactionReadOnlyRepository transactionReadOnlyRepository)
        : IRequestHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var transaction = await transactionReadOnlyRepository.GetSingleAsync(
                transaction => transaction.Id == request.Id, TransactionResponse.Projection);

            return transaction ?? throw new NotFoundException(Generals.TransactionNotFound);
        }
    }
}
