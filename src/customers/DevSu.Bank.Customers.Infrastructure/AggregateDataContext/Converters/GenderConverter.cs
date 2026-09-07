using DevSu.Bank.Customers.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevSu.Bank.Customers.Infrastructure.AggregateDataContext.Converters
{
    public class GenderConverter : ValueConverter<Gender, byte>
    {
        public GenderConverter() : base(gender => (byte)gender, value => (Gender)value)
        {
        }
    }
}
