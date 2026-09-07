using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Application.SeedWork
{
    public class QueryOptions
    {
        public string? Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
