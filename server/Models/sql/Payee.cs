using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("payees", Schema = "dbo")]
  public partial class Payee
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int payee_id
    {
      get;
      set;
    }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Bill> Bills { get; set; }
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; }
    public string name
    {
      get;
      set;
    }
  }
}
