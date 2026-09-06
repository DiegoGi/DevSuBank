namespace DevSu.Bank.Customers.Application.Services.Infrastructure
{
    public interface IPasswordHasherService
    {
        string Hash(string password);
    }
}
