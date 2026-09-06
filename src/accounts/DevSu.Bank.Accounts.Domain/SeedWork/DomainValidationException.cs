namespace DevSu.Bank.Accounts.Domain.SeedWork
{
    public class DomainValidationException : BaseException
    {
        public DomainValidationException(string message) : base(message)
        { }
        public DomainValidationException(string message, IEnumerable<string> details) : base(message, details)
        { }
    }
}
