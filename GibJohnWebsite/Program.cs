using GibJohnWebsite.Data;
using GibJohnWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GibJohnWebsiteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GibJohnWebsiteContext") ?? throw new InvalidOperationException("Connection string 'GibJohnWebsiteContext' not found.")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Ensure role management is added here
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Ensure this uses ApplicationDbContext
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create roles and seed courses
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles(services);
    await SeedCourses(services);
}

app.Run();

async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Admin", "Tutor", "Student"};
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Create the roles and seed them to the database
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task SeedCourses(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<GibJohnWebsiteContext>();

    // Check if any courses exist
    if (!context.CoursesClass.Any())
    {
        // Add initial courses
        var courses = new List<CoursesClass>
        {
            new CoursesClass { Title = "English", Description = "Shakespeare?"},
            new CoursesClass { Title = "Mathematics", Description = "Try and be einstein i suppose."},
            new CoursesClass { Title = "Sciences", Description = "Learn to blow things up (probably)."}
        };

        context.CoursesClass.AddRange(courses);
        await context.SaveChangesAsync();
    }
}