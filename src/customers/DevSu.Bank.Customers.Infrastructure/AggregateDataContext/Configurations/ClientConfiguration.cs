using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Infrastructure.AggregateDataContext.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevSu.Bank.Customers.Infrastructure.AggregateDataContext.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("Clients");

            entity.HasKey(client => client.Id);
            entity.Property(client => client.Id).ValueGeneratedOnAdd();

            entity.Property(client => client.Name).HasMaxLength(150).IsRequired();
            entity.Property(client => client.Gender).HasConversion<GenderConverter>().HasColumnType("tinyint").IsRequired();
            entity.Property(client => client.Age).IsRequired();
            entity.Property(client => client.Identification).HasMaxLength(30).IsRequired();
            entity.Property(client => client.Address).HasMaxLength(250);
            entity.Property(client => client.Phone).HasMaxLength(30);
            entity.Property(client => client.ClientId).HasMaxLength(20).IsRequired();
            entity.Property(client => client.PasswordHash).HasMaxLength(200).IsRequired();
            entity.Property(client => client.Status).IsRequired();

            entity.HasIndex(client => client.ClientId, "UQ_Clients_ClientId").IsUnique();
            entity.HasIndex(client => client.Identification, "UQ_Clients_Identification").IsUnique();
        }
    }
}
