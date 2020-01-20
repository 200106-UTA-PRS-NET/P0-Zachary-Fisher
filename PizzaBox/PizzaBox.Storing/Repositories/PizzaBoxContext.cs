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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK__customer__DD701264A672C2FD");

                entity.ToTable("customer");

                entity.HasIndex(e => e.Uname)
                    .HasName("UQ__customer__C7D2484ED9C42ED8")
                    .IsUnique();

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.Lastorder)
                    .HasColumnName("lastorder")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasColumnName("pass")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Uname)
                    .IsRequired()
                    .HasColumnName("uname")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Orders__C2FFCF1304D706B9");

                entity.Property(e => e.Oid).HasColumnName("oid");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Pizzas)
                    .HasColumnName("pizzas")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Sid).HasColumnName("sid");

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Sid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__sid__4F7CD00D");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__uid__5070F446");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Sid)
                    .HasName("PK__STORE__DDDFDD3646E29414");

                entity.ToTable("STORE");

                entity.HasIndex(e => e.Sname)
                    .HasName("UQ__STORE__0F1ED583E756EC7A")
                    .IsUnique();

                entity.Property(e => e.Sid).HasColumnName("sid");

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasColumnName("sname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Spass)
                    .IsRequired()
                    .HasColumnName("spass")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
