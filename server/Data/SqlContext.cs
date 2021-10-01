using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using PersonalFinance.Models.Sql;

namespace PersonalFinance.Data
{
  public partial class SqlContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public SqlContext(DbContextOptions<SqlContext> options):base(options)
    {
    }

    public SqlContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PersonalFinance.Models.Sql.Balance>().HasNoKey();
        builder.Entity<PersonalFinance.Models.Sql.SpendingThisMonth>().HasNoKey();
        builder.Entity<PersonalFinance.Models.Sql.SummaryCategoriesThisMonth>().HasNoKey();
        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .HasOne(i => i.Payee)
              .WithMany(i => i.Bills)
              .HasForeignKey(i => i.payee_id)
              .HasPrincipalKey(i => i.payee_id);
        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .HasOne(i => i.Category)
              .WithMany(i => i.Bills)
              .HasForeignKey(i => i.category_id)
              .HasPrincipalKey(i => i.category_id);
        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .HasOne(i => i.Status)
              .WithMany(i => i.Bills)
              .HasForeignKey(i => i.status_id)
              .HasPrincipalKey(i => i.status_id);
        builder.Entity<PersonalFinance.Models.Sql.Budget>()
              .HasOne(i => i.Category)
              .WithMany(i => i.Budgets)
              .HasForeignKey(i => i.category_id)
              .HasPrincipalKey(i => i.category_id);
        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .HasOne(i => i.Payee)
              .WithMany(i => i.RecurringTransactions)
              .HasForeignKey(i => i.payee_id)
              .HasPrincipalKey(i => i.payee_id);
        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .HasOne(i => i.Account)
              .WithMany(i => i.Transactions)
              .HasForeignKey(i => i.account_id)
              .HasPrincipalKey(i => i.account_id);
        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .HasOne(i => i.Category)
              .WithMany(i => i.Transactions)
              .HasForeignKey(i => i.category_id)
              .HasPrincipalKey(i => i.category_id);
        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .HasOne(i => i.Payee)
              .WithMany(i => i.Transactions)
              .HasForeignKey(i => i.payee_id)
              .HasPrincipalKey(i => i.payee_id);
        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .HasOne(i => i.Status)
              .WithMany(i => i.Transactions)
              .HasForeignKey(i => i.status_id)
              .HasPrincipalKey(i => i.status_id);

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.due)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.status_id)
              .HasDefaultValueSql("((1))");

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.account_id)
              .HasDefaultValueSql("((1))");


        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.due)
              .HasColumnType("date");

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.date)
              .HasColumnType("date");

        builder.Entity<PersonalFinance.Models.Sql.Account>()
              .Property(p => p.account_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Balance>()
              .Property(p => p.balance1)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.bill_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.payee_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.amount)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.Bill>()
              .Property(p => p.status_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Budget>()
              .Property(p => p.budget_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Budget>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Budget>()
              .Property(p => p.amount)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.Category>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.DefaultCategory>()
              .Property(p => p.payee_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.DefaultCategory>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Payee>()
              .Property(p => p.payee_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.bill_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.payee_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.amount)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.frequency)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.year)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.month)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.RecurringTransaction>()
              .Property(p => p.day)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.SpendingThisMonth>()
              .Property(p => p.budget_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.SpendingThisMonth>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.SpendingThisMonth>()
              .Property(p => p.budgeted)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.SpendingThisMonth>()
              .Property(p => p.spent)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.Status>()
              .Property(p => p.status_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Status>()
              .Property(p => p.type)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.SummaryCategoriesThisMonth>()
              .Property(p => p.total)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.transaction_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.account_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.checkNumber)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.category_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.payee_id)
              .HasPrecision(10, 0);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.amount)
              .HasPrecision(19, 4);

        builder.Entity<PersonalFinance.Models.Sql.Transaction>()
              .Property(p => p.status_id)
              .HasPrecision(10, 0);
        this.OnModelBuilding(builder);
    }


    public DbSet<PersonalFinance.Models.Sql.Account> Accounts
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Balance> Balances
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Bill> Bills
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Budget> Budgets
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Category> Categories
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.DefaultCategory> DefaultCategories
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Payee> Payees
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.RecurringTransaction> RecurringTransactions
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.SpendingThisMonth> SpendingThisMonths
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Status> Statuses
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.SummaryCategoriesThisMonth> SummaryCategoriesThisMonths
    {
      get;
      set;
    }

    public DbSet<PersonalFinance.Models.Sql.Transaction> Transactions
    {
      get;
      set;
    }
  }
}
