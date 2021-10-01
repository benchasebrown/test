using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models.Sql
{
  [Table("bills", Schema = "dbo")]
  public partial class Bill
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public string description
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
    public DateTime due
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
