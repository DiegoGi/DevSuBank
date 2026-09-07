using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate.Specifications;
using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.DeactivateClient
{
    public class DeactivateClientCommandHandler(
        IClientRepository clientRepository,
        IAccountRepository accountRepository)
        : IRequestHandler<DeactivateClientCommand, Unit>
    {
        public async Task<Unit> Handle(DeactivateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetSingleByIdAsync(request.Id);

            if (client is null)
            {
                return Unit.Value;
            }

            client.Update(client.Name, false);
            clientRepository.Update(client);

            var accounts = await accountRepository.ListAsync(new AccountsByClientSpecification(request.Id),
                account => account);

            foreach (var account in accounts)
            {
                account.Deactivate();
                accountRepository.Update(account);
            }

            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
