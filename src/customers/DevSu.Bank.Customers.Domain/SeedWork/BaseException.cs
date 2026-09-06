namespace DevSu.Bank.Customers.Domain.SeedWork
{
    public abstract class BaseException : Exception
    {
        protected BaseException(string message) : base(message)
        {
        }
        protected BaseException(string message, IEnumerable<string> details) : base(message)
        {
            Details = details;
        }

        protected BaseException(string message, string detail) : base(message)
        {
            Details = new List<string>
            {
                detail
            };
        }

        public IEnumerable<string> Details { get; protected set; } = [];
    }
}
