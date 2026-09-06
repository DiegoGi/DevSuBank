
using DevSu.Bank.Customers.Application.ReadOnlyModels;
using DevSu.Bank.Customers.Infrastructure.ReadOnlyDataContext.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace DevSu.Bank.Customers.Infrastructure.ReadOnlyDataContext;

public partial class ReadOnlyContext : DbContext
{
    public ReadOnlyContext(DbContextOptions<ReadOnlyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.ClientConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
