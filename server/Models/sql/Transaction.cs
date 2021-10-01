using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("transactions", Schema = "dbo")]
  public partial class Transaction
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int transaction_id
    {
      get;
      set;
    }
    public int account_id
    {
      get;
      set;
    }
    public Account Account { get; set; }
    public DateTime date
    {
      get;
      set;
    }
    public int? checkNumber
    {
      get;
      set;
    }
    public int category_id
    {
      get;
      set;
    }
    public Category Category { get; set; }
    public int payee_id
    {
      get;
      set;
    }
    public Payee Payee { get; set; }
    public string description
    {
      get;
      set;
    }
    public decimal? amount
    {
      get;
      set;
    }
    public int status_id
    {
      get;
      set;
    }
    public Status Status { get; set; }
  }
}
