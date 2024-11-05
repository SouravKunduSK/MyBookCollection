
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBookCollection.Models;
using MyBookCollection.Models.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure Database

builder.Services.AddDbContext<MyBookCollectionDbContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Configure Authentication

// 1. Adding Identity Service
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<MyBookCollectionDbContext>()
    .AddDefaultTokenProviders();

// 2. Configure cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Authentication/Login";
    options.SlidingExpiration = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seed the database
DbInitializer.SeedDefaultUsersAndRolesAsync(app).Wait();

app.Run();
