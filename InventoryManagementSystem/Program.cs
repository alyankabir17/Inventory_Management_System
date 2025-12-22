using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // Enable runtime view compilation

// Registering ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2. Add Session Services (Only need to do this ONCE)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: Set timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//A helper tool that allows your .cshtml views or other classes to 
//access the "Session" or "Logged-in User" data easily.
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Seed hardcoded admin account
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // Apply any pending migrations
    context.Database.Migrate();
    
    // Check if admin exists
    if (!context.Staff.Any(u => u.Username == "admin"))
    {
        context.Staff.Add(new InventoryManagementSystem.Models.Entities.Staff
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = "admin@ims.com",
            Username = "admin",
            Password = "admin123", // In production, this should be hashed
            Role = "Admin" // Set admin role
        });
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Load images/css first (so Session doesn't run on them)

app.UseRouting(); // Determine which controller handles the request

// 3. Enable Session Middleware (Must be AFTER UseRouting and BEFORE MapControllerRoute)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();