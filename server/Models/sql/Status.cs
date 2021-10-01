using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("statuses", Schema = "dbo")]
  public partial class Status
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int status_id
    {
      get;
      set;
    }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Bill> Bills { get; set; }
    public int? type
    {
      get;
      set;
    }
    public string name
    {
      get;
      set;
    }
  }
}
