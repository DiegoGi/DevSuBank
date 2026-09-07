using DevSu.Bank.Customers.Domain.SeedWork;

namespace DevSu.Bank.Customers.Application.SeedWork
{
    public class NotFoundException(string message) : BaseException(message);
}
