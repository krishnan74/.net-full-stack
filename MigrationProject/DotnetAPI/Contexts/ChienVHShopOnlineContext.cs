using ChienVHShopOnline.Models;
using ChienVHShopOnline.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChienVHShopOnline.Contexts
{
    public class ChienVHShopOnlineContext : DbContext
    {
        public ChienVHShopOnlineContext(DbContextOptions<ChienVHShopOnlineContext> options) : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        // public virtual DbSet<Color> Colors { get; set; }
        // public virtual DbSet<Model> Models { get; set; }
        // public virtual DbSet<News> News { get; set; }
        // public virtual DbSet<Order> Orders { get; set; }
        // public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        // public virtual DbSet<Product> Products { get; set; }
        // public virtual DbSet<User> Users { get; set; }
        // public virtual DbSet<ContactU> ContactUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}