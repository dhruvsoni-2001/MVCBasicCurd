// Data/SeedData.cs
using MVCBasicCurd.Models;
using CompleteBanking_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MVCBasicCurd.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeedDataAsync(BankingDbContext db)
        {
            // Apply pending migrations if you call this before update (optional)
            // await db.Database.MigrateAsync();

            if (!await db.Roles.AnyAsync())
            {
                var super = new Role { Name = "SuperAdmin", Description = "Full access" };
                var admin = new Role { Name = "Admin", Description = "Admin user" };
                var user = new Role { Name = "User", Description = "Regular user" };
                db.Roles.AddRange(super, admin, user);
                await db.SaveChangesAsync();
            }

            var superAdminEmail = "superadmin@bank.local";
            var existing = await db.Users.FirstOrDefaultAsync(u => u.Email == superAdminEmail);
            if (existing == null)
            {
                var su = new User
                {
                    FullName = "Super Admin",
                    Email = superAdminEmail,
                    PhoneNumber = "9999999999",
                    Address = "Local",
                    BusinessUserId = $"SUPER-{DateTime.UtcNow:yyyyMMddHHmmss}"
                };

                db.Users.Add(su);
                await db.SaveChangesAsync();

                var superRole = await db.Roles.FirstAsync(r => r.Name == "SuperAdmin");
                db.UserRoles.Add(new UserRole { UserId = su.UserId, RoleId = superRole.RoleId });
                await db.SaveChangesAsync();
            }
        }
    }
}
