using EFLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFLibrary {
    public class AppDbContext : DbContext {
        public virtual DbSet<Customer> Customers { get; set; }//start of the list of the classes that map to a table. If you want to access the table you must add one of these lines for each.
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Orderline> Orderlines { get; set; }
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            if (!builder.IsConfigured) {
                builder.UseLazyLoadingProxies();
                var connStr = @"server=localhost\sqlexpress;database=CustEfDb;trusted_connection=true;";
                builder.UseSqlServer(connStr);
            }
        }
        protected override void OnModelCreating(ModelBuilder model) {
            model.Entity<Product>(e => {
                e.ToTable("Products");
                e.HasKey(x => x.Id);
                e.Property(x => x.Code).HasMaxLength(10).IsRequired();
                e.Property(x => x.Name).HasMaxLength(30).IsRequired();
                e.Property(x => x.Price);
                e.HasIndex(x => x.Code).IsUnique();
            });
            model.Entity<Orderline>(e => {
                e.ToTable("Orderlines");
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Product).WithMany(x => x.Orderlines).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
