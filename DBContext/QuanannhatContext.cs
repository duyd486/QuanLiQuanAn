﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuanLiQuanAn.Models;

namespace QuanLiQuanAn.DBContext;

public partial class QuanannhatContext : DbContext
{
    public QuanannhatContext()
    {
    }

    public QuanannhatContext(DbContextOptions<QuanannhatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<Dishlist> Dishlists { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Information> Informations { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientBill> IngredientBills { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderBill> OrderBills { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<SalaryBill> SalaryBills { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-0333H5R\\SQLEXPRESS;Initial Catalog=quanannhat;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__discount__3213E83FE6A6F747");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.Discounts).HasConstraintName("FK__discounts__dish___5070F446");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dishes__3213E83FCAC1C1AA");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dishlist).WithMany(p => p.Dishes).HasConstraintName("FK__dishes__dishlist__5165187F");
        });

        modelBuilder.Entity<Dishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dishlist__3213E83F5073668A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FA2005E23");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Information).WithMany(p => p.Employees).HasConstraintName("FK__employees__infor__52593CB8");
        });

        modelBuilder.Entity<Information>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__informat__3213E83FC360AF55");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83F912D214C");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IngredientBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingredie__3213E83FFB287E69");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientBills).HasConstraintName("FK__ingredien__ingre__534D60F1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83FBC1F099F");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.Orders).HasConstraintName("FK__orders__dish_id__571DF1D5");

            entity.HasOne(d => d.Orderbill).WithMany(p => p.Orders).HasConstraintName("FK__orders__orderbil__5812160E");
        });

        modelBuilder.Entity<OrderBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_bi__3213E83F83C32B48");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Time)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Discount).WithMany(p => p.OrderBills).HasConstraintName("FK__order_bil__disco__5441852A");

            entity.HasOne(d => d.Table).WithMany(p => p.OrderBills).HasConstraintName("FK__order_bil__table__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.OrderBills).HasConstraintName("FK__order_bil__user___5629CD9C");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rates__3213E83FAEDE97D3");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Rates).HasConstraintName("FK__rates__user_id__59063A47");
        });

        modelBuilder.Entity<SalaryBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__salary_b__3213E83FF62F40C4");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.SalaryBills).HasConstraintName("FK__salary_bi__emplo__59FA5E80");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tables__3213E83FAD57F26A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F61BE925A");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Information).WithMany(p => p.Users).HasConstraintName("FK__users__informati__5AEE82B9");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__wishlist__3213E83FFC6411CE");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.Wishlists).HasConstraintName("FK__wishlist__dish_i__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists).HasConstraintName("FK__wishlist__user_i__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
