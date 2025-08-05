using DotnetAPI.Models;
using DotnetAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Contexts
{
    public class DotnetAPIContext : DbContext
    {
        public DotnetAPIContext(DbContextOptions<DotnetAPIContext> options) : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify primary keys
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Color>().HasKey(c => c.ColorId);
            modelBuilder.Entity<News>().HasKey(n => n.NewsId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderID);
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderID, od.ProductID });
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<ContactUs>().HasKey(cu => cu.Id);
            modelBuilder.Entity<Cart>().HasKey(c => c.CartId);
            modelBuilder.Entity<Model>().HasKey(m => m.ModelId);



            // Category - Product (one-to-many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Color - Product (one-to-many)
            modelBuilder.Entity<Product>()
                .HasOne<Color>("Color")
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ColorId)
                .OnDelete(DeleteBehavior.SetNull);

            // User - Product (one-to-many)
            modelBuilder.Entity<Product>()
                .HasOne<User>("User")
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // User - News (one-to-many)
            modelBuilder.Entity<News>()
                .HasOne(n => n.User)
                .WithMany(u => u.News)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Order - OrderDetail (one-to-many)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - OrderDetail (one-to-many)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // Cart - User (one-to-one)
            modelBuilder.Entity<Cart>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cart - CartItem (one-to-many, owned)
            modelBuilder.Entity<Cart>()
                .OwnsMany(c => c.Items, a =>
                {
                    a.WithOwner().HasForeignKey("CartId");
                    a.Property<int>("CartItemId").ValueGeneratedOnAdd();
                    a.HasKey("CartItemId");
                });

            // Model - Product (one-to-many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Model)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ModelId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}