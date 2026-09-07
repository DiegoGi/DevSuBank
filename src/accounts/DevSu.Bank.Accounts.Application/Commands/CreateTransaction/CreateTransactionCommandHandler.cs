using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler(IAccountRepository accountRepository)
        : IRequestHandler<CreateTransactionCommand, long>
    {
        public async Task<long> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetSingleAsync(
                account => account.AccountNumber == request.AccountNumber, account => account)
                ?? throw new NotFoundException(Generals.AccountNotFound);

            var transaction = account.RegisterTransaction(request.Amount);

            accountRepository.Update(account);

            await accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}
