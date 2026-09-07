using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record TransactionResponse(
        [property: JsonPropertyName("id")] long Id,
        [property: JsonPropertyName("fecha")] DateTime TransactionDate,
        [property: JsonPropertyName("tipoMovimiento")] TransactionType TransactionType,
        [property: JsonPropertyName("valor")] decimal Amount,
        [property: JsonPropertyName("saldo")] decimal Balance,
        [property: JsonPropertyName("numeroCuenta")] string AccountNumber)
    {
        public static readonly Expression<Func<Transaction, TransactionResponse>> Projection =
            transaction => new TransactionResponse(transaction.Id, transaction.TransactionDate,
                (TransactionType)transaction.TransactionType, transaction.Amount, transaction.Balance,
                transaction.Account.AccountNumber);
    }
}
