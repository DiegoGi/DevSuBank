using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Application.SeedWork
{
    public class NotFoundException(string message) : BaseException(message);
}
