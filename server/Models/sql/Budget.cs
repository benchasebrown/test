using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("budgets", Schema = "dbo")]
  public partial class Budget
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public Category Category { get; set; }
    public decimal amount
    {
      get;
      set;
    }
  }
}
