using DataAccessObject;
using Microsoft.AspNetCore.Authentication.Cookies;
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

builder.Services.AddDbContext<KoiFarmShopDatabaseContext>(options =>
    options.UseSqlServer(context.GetConnectionString()));


builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IConsignmentRepository, ConsignmentRepository>();

builder.Services.AddScoped<IBlogRepository, BlogRepository>();

builder.Services.AddSingleton<IVnPayService, VnPayService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";  // ???ng d?n ??n trang ??ng nh?p
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Customer/Index");
    await Task.CompletedTask;
});

app.Run();
