using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext
{
    public partial class AggregateContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
