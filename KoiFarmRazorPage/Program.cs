using DataAccessObject;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository.Repository;
using System.Configuration;
using KoiFarmRazorPage.Service;

var builder = WebApplication.CreateBuilder(args);

var context = new KoiFarmShopDatabaseContext();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<KoiFarmShopDatabaseContext>(options =>
    options.UseSqlServer(context.GetConnectionString()));

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();

builder.Services.AddScoped<IConsignmentRepository, ConsignmentRepository>();

builder.Services.AddScoped<IBlogRepository, BlogRepository>();

builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirect to login page
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware
app.UseSession(); // Add this line to enable session

app.UseAuthentication(); // Ensure authentication middleware is added before authorization
app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Customer/Index");
    await Task.CompletedTask;
});

app.Run();
