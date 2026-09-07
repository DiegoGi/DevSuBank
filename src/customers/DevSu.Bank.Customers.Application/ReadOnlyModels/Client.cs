namespace DevSu.Bank.Customers.Application.ReadOnlyModels;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    /// <summary>
    /// 1 = Male, 2 = Female, 3 = Other
    /// </summary>
    public byte Gender { get; set; }

    public int Age { get; set; }

    public string Identification { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string ClientId { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool Status { get; set; }
}
