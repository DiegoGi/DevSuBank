using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext
{
    public partial class AggregateContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
