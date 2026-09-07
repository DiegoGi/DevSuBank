namespace DevSu.Bank.Accounts.Application.ReadOnlyModels;

public partial class Account
{
    public int Id { get; set; }

    public string AccountNumber { get; set; } = null!;

    /// <summary>
    /// 1 = Savings, 2 = Checking
    /// </summary>
    public byte AccountType { get; set; }

    public decimal InitialBalance { get; set; }

    public decimal CurrentBalance { get; set; }

    public bool Status { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
