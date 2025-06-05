using Notify.Models;
using Microsoft.EntityFrameworkCore;

namespace Notify.Contexts
{
    public class NotifyContext : DbContext
    {
        public NotifyContext(DbContextOptions<NotifyContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<HRAdmin> HRAdmins { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<DocumentAccess> DocumentAccesses { get; set; } = null!;
        public DbSet<FileDetails> FileDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.UploadedBy)
                .WithMany(u => u.UploadedDocuments)
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocumentAccess>()
                .HasOne(da => da.Document)
                .WithMany(d => d.DocumentAccesses)
                .HasForeignKey(da => da.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DocumentAccess>()
                .HasOne(da => da.User)
                .WithMany(u => u.DocumentAccesses)
                .HasForeignKey(da => da.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}