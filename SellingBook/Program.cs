using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SellingBook.Models;
using SellingBook.Models.Identity;
using SellingBook.Repositories;
using SellingBook.Services.Email;
using SellingBook.Services.User;
using SellingBook.Services.VNPay;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure services

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Localization first
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "vi-VN" }; // Hỗ trợ tiếng Anh và tiếng Việt
    options.DefaultRequestCulture = new RequestCulture("vi-VN"); // Mặc định là tiếng Việt
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
});


// Identity with EF
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Register your repositories
builder.Services.AddScoped<ICartRepository, EFCartRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// Add Services
builder.Services.AddScoped<IVNPayService, VNPayService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("https://localhost:7250")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// Razor Pages (for Identity UI)
builder.Services.AddRazorPages();

var app = builder.Build();
var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

// 2) Configure middleware pipeline

// Production exception handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS & static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// CORS
app.UseCors("CorsPolicy");

// Session
app.UseSession();

// Routing
app.UseRouting();

// Authentication + Authorization for Identity
app.UseAuthentication();
app.UseAuthorization();

// 3) Map your routes
// Optionally define an admin area route
// Area route (for both Admin and Customer areas)
// Admin & Employee Area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Custom route for search
app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Product", action = "Search" }
);

// Default Route — Only HomeController is exposed to Anonymous users by default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Razor Pages (for Identity UI)
app.MapRazorPages();

app.Run();
