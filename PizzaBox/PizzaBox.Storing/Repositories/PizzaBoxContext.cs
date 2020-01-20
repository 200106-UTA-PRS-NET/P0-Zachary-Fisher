using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PizzaBox.Storing.Repositories
{
    public partial class PizzaBoxContext : DbContext
    {
        public PizzaBoxContext()
        {
        }

        public PizzaBoxContext(DbContextOptions<PizzaBoxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Store> Store { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PizzaBox;trusted_connection=TRUE");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Uname)
                    .HasName("PK__customer__C7D2484FAE2BED2F");

                entity.ToTable("customer");

                entity.Property(e => e.Uname)
                    .HasColumnName("uname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Lastorder)
                    .HasColumnName("lastorder")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Pass)
                    .HasColumnName("pass")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__orders__C2FFCF13C02E3B5A");

                entity.ToTable("orders");

                entity.Property(e => e.Oid).HasColumnName("oid");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Pizzas)
                    .HasColumnName("pizzas")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Sname)
                    .HasColumnName("sname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Uname)
                    .HasColumnName("uname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.SnameNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Sname)
                    .HasConstraintName("FK__orders__sname__60A75C0F");

                entity.HasOne(d => d.UnameNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Uname)
                    .HasConstraintName("FK__orders__uname__619B8048");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Sname)
                    .HasName("PK__store__0F1ED582CD3222F3");

                entity.ToTable("store");

                entity.Property(e => e.Sname)
                    .HasColumnName("sname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Spass)
                    .HasColumnName("spass")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
