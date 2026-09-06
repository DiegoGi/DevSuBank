using DevSu.Bank.Customers.Domain.SeedWork;

namespace DevSu.Bank.Customers.Application.SeedWork
{
    public class ApplicationValidationException : BaseException
    {
        public ApplicationValidationException(string message) : base(message)
        {
        }

        public ApplicationValidationException(string message, string description) : base(message, description)
        {
        }

        public ApplicationValidationException(string message, IEnumerable<string> details) : base(message)
        {
            Details = details;
        }
    }
}
