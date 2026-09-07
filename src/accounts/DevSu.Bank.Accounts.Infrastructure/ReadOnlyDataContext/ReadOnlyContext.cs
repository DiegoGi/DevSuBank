
using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using Microsoft.EntityFrameworkCore;
namespace DevSu.Bank.Accounts.Infrastructure.ReadOnlyDataContext;

public partial class ReadOnlyContext : DbContext
{
    public ReadOnlyContext(DbContextOptions<ReadOnlyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.AccountConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ClientConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TransactionConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
