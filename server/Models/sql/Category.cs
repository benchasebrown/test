using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("categories", Schema = "dbo")]
  public partial class Category
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int category_id
    {
      get;
      set;
    }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Bill> Bills { get; set; }
    public ICollection<Budget> Budgets { get; set; }
    public string name
    {
      get;
      set;
    }
  }
}
