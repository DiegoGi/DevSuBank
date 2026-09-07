using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> entity)
        {
            entity.ToTable("Transactions");

            entity.HasKey(transaction => transaction.Id);
            entity.Property(transaction => transaction.Id).ValueGeneratedOnAdd();

            entity.Property(transaction => transaction.TransactionDate).HasColumnType("datetime2(3)").IsRequired();
            entity.Property(transaction => transaction.TransactionType).HasConversion<TransactionTypeConverter>().HasColumnType("tinyint").IsRequired();
            entity.Property(transaction => transaction.Amount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(transaction => transaction.Balance).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(transaction => transaction.AccountId).IsRequired();
        }
    }
}
