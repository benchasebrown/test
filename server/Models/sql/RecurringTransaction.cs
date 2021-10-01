using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("recurringTransactions", Schema = "dbo")]
  public partial class RecurringTransaction
  {
    [Key]
    public int bill_id
    {
      get;
      set;
    }
    public string name
    {
      get;
      set;
    }
    public int? category_id
    {
      get;
      set;
    }
    public int payee_id
    {
      get;
      set;
    }
    public Payee Payee { get; set; }
    public decimal? amount
    {
      get;
      set;
    }
    public int frequency
    {
      get;
      set;
    }
    public int? year
    {
      get;
      set;
    }
    public int? month
    {
      get;
      set;
    }
    public int day
    {
      get;
      set;
    }
  }
}
