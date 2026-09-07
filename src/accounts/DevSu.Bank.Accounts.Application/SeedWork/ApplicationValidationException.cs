using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Application.SeedWork
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
