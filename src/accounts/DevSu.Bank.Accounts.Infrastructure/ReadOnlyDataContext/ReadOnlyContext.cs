using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Accounts.Infrastructure.ReadOnlyDataContext;

public partial class ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
