using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace EcommerceCommon.OrderModels
{
    public partial class DbContextOrders : DbContext
    {
        //### Automatically create by using Scaffold-DbContext command###
        //### Scaffold-DbContext "<connectionString>" Microsoft.EntityFrameworkCore.SqlServer -OutputDir OrderModels -Context DbContextOrders ###
        public DbContextOrders(DbContextOptions<DbContextOrders> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Orderitem> Orderitems { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDERS");

                entity.Property(e => e.Orderid).HasColumnName("ORDERID");

                entity.Property(e => e.Containsgift).HasColumnName("CONTAINSGIFT");

                entity.Property(e => e.Customerid)
                    .HasMaxLength(10)
                    .HasColumnName("CUSTOMERID");

                entity.Property(e => e.Deliveryexpected)
                    .HasColumnType("date")
                    .HasColumnName("DELIVERYEXPECTED");

                entity.Property(e => e.Orderdate)
                    .HasColumnType("date")
                    .HasColumnName("ORDERDATE");

                entity.Property(e => e.Ordersource)
                    .HasMaxLength(30)
                    .HasColumnName("ORDERSOURCE");

                entity.Property(e => e.Shippingmode)
                    .HasMaxLength(30)
                    .HasColumnName("SHIPPINGMODE");
            });

            modelBuilder.Entity<Orderitem>(entity =>
            {
                entity.ToTable("ORDERITEMS");

                entity.Property(e => e.Orderitemid).HasColumnName("ORDERITEMID");

                entity.Property(e => e.Orderid).HasColumnName("ORDERID");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Productid).HasColumnName("PRODUCTID");

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.Property(e => e.Returnable).HasColumnName("RETURNABLE");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Orderitems)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK__ORDERITEM__ORDER__60A75C0F");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orderitems)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK__ORDERITEM__PRODU__619B8048");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCTS");

                entity.Property(e => e.Productid).HasColumnName("PRODUCTID");

                entity.Property(e => e.Colour)
                    .HasMaxLength(20)
                    .HasColumnName("COLOUR");

                entity.Property(e => e.Packheight)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("PACKHEIGHT");

                entity.Property(e => e.Packweight)
                    .HasColumnType("decimal(8, 3)")
                    .HasColumnName("PACKWEIGHT");

                entity.Property(e => e.Packwidth)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("PACKWIDTH");

                entity.Property(e => e.Productname)
                    .HasMaxLength(50)
                    .HasColumnName("PRODUCTNAME");

                entity.Property(e => e.Size)
                    .HasMaxLength(20)
                    .HasColumnName("SIZE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
