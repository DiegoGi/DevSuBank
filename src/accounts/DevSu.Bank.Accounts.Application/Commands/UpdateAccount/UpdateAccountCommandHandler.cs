using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.UpdateAccount
{
    public class UpdateAccountCommandHandler(IAccountRepository accountRepository)
        : IRequestHandler<UpdateAccountCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetSingleByIdAsync(request.Id)
                ?? throw new NotFoundException(Generals.AccountNotFound);

            account.ChangeAccountType(request.AccountType);

            if (request.Status)
            {
                account.Activate();
            }
            else
            {
                account.Deactivate();
            }

            accountRepository.Update(account);

            await accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
