﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiQuanAn.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("password")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column("role")]
    public int? Role { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("avatar")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Avatar { get; set; }

    [Column("information_id")]
    public int? InformationId { get; set; }

    [Column("score")]
    public int? Score { get; set; }

    [Column("total_score")]
    public int? TotalScore { get; set; }

    [ForeignKey("InformationId")]
    [InverseProperty("Users")]
    public virtual Information? Information { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();

    [InverseProperty("User")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
