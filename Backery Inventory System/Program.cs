using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BakeryInventorySystem.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<BakeryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BakeryDbContext")
        ?? throw new InvalidOperationException("Connection string 'BakeryDbContext' not found.")));

// Password hashing service
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<BakeryDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();
    db.Database.Migrate();

    var existingAdmin = db.Users.FirstOrDefault(u => u.Username == "admin");

    if (existingAdmin == null)
    {
        var admin = new AppUser
        {
            FullName = "Admin User",
            Username = "admin",
            Role = "Administrator",
            IsActive = true
        };
        admin.PasswordHash = hasher.HashPassword(admin, "admin123");
        db.Users.Add(admin);
        db.SaveChanges();
    }
    else if (existingAdmin.PasswordHash == "admin123")
    {
        // Purana plain-text password tha, ab hash kr dete hain
        existingAdmin.PasswordHash = hasher.HashPassword(existingAdmin, "admin123");
        db.SaveChanges();
    }
}
catch (Exception ex)
{
    Console.WriteLine("Seeding error: " + ex.Message);
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();