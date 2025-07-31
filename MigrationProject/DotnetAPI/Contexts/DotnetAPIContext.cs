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
        // public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<News> News { get; set; }
        // public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ContactU> ContactUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}