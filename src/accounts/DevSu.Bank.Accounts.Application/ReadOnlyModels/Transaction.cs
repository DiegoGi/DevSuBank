namespace DevSu.Bank.Accounts.Application.ReadOnlyModels;

public partial class Transaction
{
    public long Id { get; set; }

    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// 1 = Deposit, 2 = Withdrawal
    /// </summary>
    public byte TransactionType { get; set; }

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;
}
