using System;
using System.Collections.Generic;

namespace DevSu.Bank.Accounts.Application.ReadOnlyModels;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
