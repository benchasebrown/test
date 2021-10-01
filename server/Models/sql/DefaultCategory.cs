using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("default_categories", Schema = "dbo")]
  public partial class DefaultCategory
  {
    [Key]
    public int payee_id
    {
      get;
      set;
    }
    public int category_id
    {
      get;
      set;
    }
  }
}
