using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("accounts", Schema = "dbo")]
  public partial class Account
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int account_id
    {
      get;
      set;
    }

    public ICollection<Transaction> Transactions { get; set; }
    public string name
    {
      get;
      set;
    }
    public string description
    {
      get;
      set;
    }
  }
}
