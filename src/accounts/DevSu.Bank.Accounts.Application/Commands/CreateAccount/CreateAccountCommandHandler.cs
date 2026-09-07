using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.CreateAccount
{
    public class CreateAccountCommandHandler(
        IAccountRepository accountRepository,
        IClientRepository clientRepository)
        : IRequestHandler<CreateAccountCommand, int>
    {
        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            if (await accountRepository.ExistsAsync(account => account.AccountNumber == request.AccountNumber))
            {
                throw new ApplicationValidationException(Generals.AccountNumberAlreadyRegistered);
            }

            var client = await clientRepository.GetSingleByIdAsync(request.ClientId);

            if (client is null || !client.Status)
            {
                throw new ApplicationValidationException(Generals.ClientNotFound);
            }

            var account = new Account(request.AccountNumber, request.AccountType, request.InitialBalance,
                request.ClientId);

            await accountRepository.CreateAsync(account);
            await accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return account.Id;
        }
    }
}
