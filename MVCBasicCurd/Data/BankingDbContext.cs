// Data/BankingDbContext.cs
using CompleteBanking_MVC.Models;
using Microsoft.EntityFrameworkCore;
using MVCBasicCurd.Models;

namespace MVCBasicCurd.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Confirmation> Confirmations => Set<Confirmation>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Existing user/account configuration (keep yours)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.BusinessUserId).IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber).IsUnique();

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //// Role - UserRole relations
            //modelBuilder.Entity<UserRole>()
            //    .HasOne(ur => ur.User)
            //    .WithMany(u => u.Accounts /* placeholder? see note below */);

            // Proper config for the many-to-many:
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name).IsUnique();

            // Transactions and AccountNumber alternate key as you already had...
            modelBuilder.Entity<Account>()
                .HasAlternateKey(a => a.AccountNumber)
                .HasName("AK_Account_AccountNumber");

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.AccountNumber);

            modelBuilder.Entity<Transaction>()
                .HasOne<Account>()
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Confirmation>()
                .HasIndex(c => new { c.Type, c.AccountNumber });
        }

    }
}
