﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiQuanAn.Models;

[Table("tables")]
public partial class Table
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("contain")]
    public int? Contain { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [InverseProperty("Table")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
