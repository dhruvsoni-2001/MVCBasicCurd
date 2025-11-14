using MVCBasicCurd.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- ADD DB CONTEXT (SQL Server LocalDB example) ---
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// If you later use Identity, add it here. For now we keep it simple.

// Build the app
var app = builder.Build();

// Run seed if you have a seed helper (ensure DB exists / migrations applied first)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    // Only call seed if you know migrations are applied; otherwise call after migrations
    // await SeedData.EnsureSeedDataAsync(db);
    db.Database.Migrate();
    // run seeding
    await SeedData.EnsureSeedDataAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
