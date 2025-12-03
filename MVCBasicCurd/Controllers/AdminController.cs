using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBasicCurd.Data;
using MVCBasicCurd.DTOs;
using MVCBasicCurd.Models;
using MVCBasicCurd.Services; // Ensure AccountNumberGenerator is here

namespace MVCBasicCurd.Controllers
{
    public class AdminController : Controller
    {
        private readonly BankingDbContext _db;

        public AdminController(BankingDbContext db) => _db = db;

        // =========================================================
        // GET: /Admin/Index (Read)
        // =========================================================
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                // Include Accounts so we can show balance in the list if needed
                .Include(u => u.Accounts)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        // =========================================================
        // GET: /Admin/Create
        // =========================================================
        public IActionResult Create() => View();

        // =========================================================
        // POST: /Admin/Create (Create User + Auto Account)
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDto input)
        {
            if (!ModelState.IsValid) return View(input);

            if (await _db.Users.AnyAsync(u => u.Email == input.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(input);
            }

            // Start a transaction to ensure User, Role, and Account are created together
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // 1. Prepare the User Object
                var user = new User
                {
                    FullName = input.FullName,
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber,
                    Address = input.Address,
                    BusinessUserId = $"USER-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    CreatedAt = DateTime.UtcNow
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync(); // Save to get the generated UserId

                // 2. Assign "User" Role
                var userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (userRole != null)
                {
                    _db.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = userRole.RoleId });
                }

                // 3. AUTOMATIC ACCOUNT CREATION
                // Generate a unique account number
                string newAccNumber = AccountNumberGenerator.Generate();

                var newAccount = new Account
                {
                    UserId = user.UserId,
                    AccountNumber = newAccNumber,
                    Type = AccountType.Savings, // Default to Savings
                    Balance = ,               // Opening balance
                    NickName = "Primary Savings",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Accounts.Add(newAccount);

                // 4. Update the User's "Primary Account" reference (optional, based on your model)
                user.AccountNumber = newAccNumber;
                _db.Users.Update(user);

                await _db.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                Console.WriteLine($"Success: User {user.UserId} created with Account {newAccNumber}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error: " + ex.Message);
                ModelState.AddModelError("", "System error creating user/account. Please try again.");
                return View(input);
            }
        }

        // =========================================================
        // GET: /Admin/Edit/{id}
        // =========================================================
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Map Entity to DTO
            var dto = new UserEditDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(dto);
        }

        // =========================================================
        // POST: /Admin/Edit/{id}
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserEditDto input)
        {
            if (id != input.UserId) return NotFound();

            if (!ModelState.IsValid) return View(input);

            // Fetch existing user to update
            var userToUpdate = await _db.Users.FindAsync(id);
            if (userToUpdate == null) return NotFound();

            // Update fields
            userToUpdate.FullName = input.FullName;
            userToUpdate.Email = input.Email;
            userToUpdate.PhoneNumber = input.PhoneNumber;
            userToUpdate.Address = input.Address;
            userToUpdate.UpdatedAt = DateTime.UtcNow;

            try
            {
                _db.Users.Update(userToUpdate);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
                return View(input);
            }
        }

        // =========================================================
        // GET: /Admin/Delete/{id}
        // =========================================================
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _db.Users
                .Include(u => u.Accounts) // Load accounts to warn user about deleting them
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null) return NotFound();

            return View(user);
        }

        // =========================================================
        // POST: /Admin/Delete/{id}
        // =========================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                // Note: If you have configured Cascade Delete in OnModelCreating, 
                // deleting the user deletes accounts automatically.
                // Otherwise, you must remove accounts manually first.

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}