using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("spendingThisMonth", Schema = "dbo")]
  public partial class SpendingThisMonth
  {
    public int budget_id
    {
      get;
      set;
    }
    public int category_id
    {
      get;
      set;
    }
    public string name
    {
      get;
      set;
    }
    public decimal budgeted
    {
      get;
      set;
    }
    public decimal? spent
    {
      get;
      set;
    }
    public string spentPercent
    {
      get;
      set;
    }
    public string remaining
    {
      get;
      set;
    }
    public string remainingPercent
    {
      get;
      set;
    }
  }
}
