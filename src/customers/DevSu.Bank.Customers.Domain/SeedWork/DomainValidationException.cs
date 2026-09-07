namespace DevSu.Bank.Customers.Domain.SeedWork
{
    public class DomainValidationException : BaseException
    {
        public DomainValidationException(string message) : base(message)
        { }
        public DomainValidationException(string message, IEnumerable<string> details) : base(message, details)
        { }
    }
}
