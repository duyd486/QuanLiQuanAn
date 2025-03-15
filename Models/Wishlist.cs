﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiQuanAn.Models;

[Table("wishlist")]
public partial class Wishlist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("dish_id")]
    public int? DishId { get; set; }

    [ForeignKey("DishId")]
    [InverseProperty("Wishlists")]
    public virtual Dish? Dish { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Wishlists")]
    public virtual User? User { get; set; }
}
