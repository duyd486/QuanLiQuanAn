using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiQuanAn.Models;

[Table("ingredients")]
public partial class Ingredient
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [InverseProperty("Ingredient")]
    public virtual ICollection<IngredientBill> IngredientBills { get; set; } = new List<IngredientBill>();
}
