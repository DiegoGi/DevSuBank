using DevSu.Bank.Accounts.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateDataContext.Converters
{
    public class TransactionTypeConverter : ValueConverter<TransactionType, byte>
    {
        public TransactionTypeConverter()
            : base(transactionType => (byte)transactionType, value => (TransactionType)value)
        {
        }
    }
}
