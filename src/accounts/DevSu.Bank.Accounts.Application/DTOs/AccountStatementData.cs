namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record AccountStatementData(DateTime TransactionDate, string ClientName, string AccountNumber,
        byte AccountType, decimal InitialBalance, bool Status, decimal Amount, decimal Balance);
}
