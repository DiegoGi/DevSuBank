using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.SeedWork
{
    public class PagedList<T>(int maxCount, IEnumerable<T> list)
    {
        [JsonPropertyName("total")]
        public int MaxCount { get; private set; } = maxCount;
        [JsonPropertyName("datos")]
        public IEnumerable<T> List { get; private set; } = list;
    }
}
