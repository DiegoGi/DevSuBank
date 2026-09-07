namespace DevSu.Bank.Customers.Domain.SeedWork
{
    public class OrderBy
    {
        public required string SortField { get; set; }
        public SortOrder Type { get; set; }
    }
}
