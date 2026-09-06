namespace DevSu.Bank.Accounts.Application.SeedWork
{
    public class PagedList<T>(int maxCount, IEnumerable<T> list)
    {
        public int MaxCount { get; private set; } = maxCount;
        public IEnumerable<T> List { get; private set; } = list;
    }
}
