using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Customers.Infrastructure.AggregateDataContext
{
    public partial class AggregateContext : DbContext
    {
        public virtual DbSet<Client> Clients { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
