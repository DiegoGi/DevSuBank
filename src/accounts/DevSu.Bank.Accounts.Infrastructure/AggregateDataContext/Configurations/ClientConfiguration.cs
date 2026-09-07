using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("Clients");

            entity.HasKey(client => client.Id);
            entity.Property(client => client.Id).ValueGeneratedNever();

            entity.Property(client => client.Name).HasMaxLength(150).IsRequired();
            entity.Property(client => client.Status).IsRequired();
        }
    }
}
