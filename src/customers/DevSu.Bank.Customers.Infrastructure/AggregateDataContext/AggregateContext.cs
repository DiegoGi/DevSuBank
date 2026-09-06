using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Customers.Infrastructure.AggregateDataContext
{
    public partial class AggregateContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
