using Microsoft.EntityFrameworkCore;
using BankApplication.Models;

namespace BankApplication.Contexts
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Bank> banks { get; set; }
        public DbSet<Branch> branches { get; set; }
        public DbSet<Transaction> transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(acc => acc.AccountNumber).HasName("PK_AccountNumber");

            modelBuilder.Entity<Branch>().HasKey(br => br.BranchCode).HasName("PK_BranchCode");

            modelBuilder.Entity<Account>().HasOne(acc => acc.User)
                .WithMany(u => u.UserAccounts)
                .HasForeignKey(acc => acc.UserId)
                .HasConstraintName("FK_Account_User")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().HasOne(acc => acc.Bank)
                .WithMany(b => b.BankAccounts)
                .HasForeignKey(acc => acc.BankId)
                .HasConstraintName("FK_Account_Bank")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Branch>().HasOne(br => br.Bank)
                .WithMany(b => b.BankBranches)
                .HasForeignKey(br => br.BankId)
                .HasConstraintName("FK_Branch_Bank")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>().HasOne(t => t.Account)
                .WithMany(acc => acc.Transactions)
                .HasForeignKey(t => t.AccountNumber)
                .HasConstraintName("FK_Transaction_Account")
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Transaction>().HasOne(t => t.TransferAccount)
                .WithMany(acc => acc.Transactions)
                .HasForeignKey(t => t.TransferAccountNumber)
                .HasConstraintName("FK_Transaction_Account")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}