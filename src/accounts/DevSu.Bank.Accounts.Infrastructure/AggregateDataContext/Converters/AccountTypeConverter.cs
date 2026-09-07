using DevSu.Bank.Accounts.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Converters
{
    public class AccountTypeConverter : ValueConverter<AccountType, byte>
    {
        public AccountTypeConverter() : base(accountType => (byte)accountType, value => (AccountType)value)
        {
        }
    }
}
