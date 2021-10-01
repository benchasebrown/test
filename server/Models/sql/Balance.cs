using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("balances", Schema = "dbo")]
  public partial class Balance
  {
    public string name
    {
      get;
      set;
    }

    [Column("balance")]
    public decimal? balance1
    {
      get;
      set;
    }
  }
}
