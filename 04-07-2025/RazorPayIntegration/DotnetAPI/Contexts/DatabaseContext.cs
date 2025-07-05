using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Contexts
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Order> orders{ get; set; }
        public DbSet<Payment> payments{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasOne(p => p.Order)
                                        .WithOne(o => o.Payment)
                                        .HasForeignKey<Payment>(p => p.OrderId)
                                        .HasConstraintName("FK_Payment_Order")
                                        .OnDelete(DeleteBehavior.Restrict);
                            
        }

    }
}