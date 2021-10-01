using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("summaryCategoriesThisMonth", Schema = "dbo")]
  public partial class SummaryCategoriesThisMonth
  {
    public string name
    {
      get;
      set;
    }
    public decimal? total
    {
      get;
      set;
    }
  }
}
