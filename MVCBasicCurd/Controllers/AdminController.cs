using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBasicCurd.Data;
using MVCBasicCurd.Models;

namespace MVCBasicCurd.Controllers
{
    // For now no authorization filter; add [Authorize] once auth exists
    public class AdminController : Controller
    {
        private readonly BankingDbContext _db;
        public AdminController(BankingDbContext db) => _db = db;

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return View(users);
        }

        // GET: /Admin/Create
        public IActionResult Create() => View();

        // POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MVCBasicCurd.DTOs.UserCreateDto input)
        {
            Console.WriteLine("AdminController.Create POST called (DTO)");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid for DTO:");
                foreach (var kv in ModelState)
                {
                    if (kv.Value.Errors.Any())
                        Console.WriteLine($" - {kv.Key}: {string.Join(", ", kv.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(input);
            }

            if (await _db.Users.AnyAsync(u => u.Email == input.Email))
            {
                ModelState.AddModelError(nameof(input.Email), "Email already exists");
                return View(input);
            }

            var user = new User
            {
                FullName = input.FullName,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Address = input.Address,
                BusinessUserId = $"USER-{DateTime.UtcNow:yyyyMMddHHmmss}"
            };

            try
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (userRole != null)
                {
                    _db.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = userRole.RoleId });
                    await _db.SaveChangesAsync();
                }

                Console.WriteLine($"User created: {user.UserId} / {user.Email}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception saving user: " + ex);
                ModelState.AddModelError("", "An error occurred saving the user. Check server logs.");
                return View(input);
            }
        }
    }
}