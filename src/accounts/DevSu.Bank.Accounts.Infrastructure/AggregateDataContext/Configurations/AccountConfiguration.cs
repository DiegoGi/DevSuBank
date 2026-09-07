using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            entity.ToTable("Accounts");

            entity.HasKey(account => account.Id);
            entity.Property(account => account.Id).ValueGeneratedOnAdd();

            entity.Property(account => account.AccountNumber).HasMaxLength(20).IsRequired();
            entity.Property(account => account.AccountType).HasConversion<AccountTypeConverter>().HasColumnType("tinyint").IsRequired();
            entity.Property(account => account.InitialBalance).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(account => account.CurrentBalance).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(account => account.Status).IsRequired();
            entity.Property(account => account.ClientId).IsRequired();

            entity.HasIndex(account => account.AccountNumber, "UQ_Accounts_AccountNumber").IsUnique();

            entity.HasMany(account => account.Transactions)
                .WithOne()
                .HasForeignKey(transaction => transaction.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Metadata
                .FindNavigation(nameof(Account.Transactions))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
