using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SellingBook.Middlewares;
using SellingBook.Models;
using SellingBook.Models.Identity;
using SellingBook.Repositories;
using SellingBook.Services;
using SellingBook.Services.ChangeLanguage;
using SellingBook.Services.Email;
using SellingBook.Services.OrderSe;
using SellingBook.Services.User;
using SellingBook.Services.VNPay;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
    options.DefaultRequestCulture = new RequestCulture("vi-VN");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews(options =>
{
    // Thêm Authorization Policy mặc định để yêu cầu người dùng đã đăng nhập
    // Nếu không có [Authorize] cụ thể, mặc định sẽ yêu cầu đăng nhập
    // options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
})
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("https://localhost:7250")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Register repositories
builder.Services.AddScoped<ICartRepository, EFCartRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<ICouponRepository, EFCouponRepository>();

// Register services
builder.Services.AddScoped<IVNPayService, VNPayService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IChangeLanguageService, ChangeLanguageService>();
builder.Services.AddScoped<GoogleDriveService>();
builder.Services.AddHttpClient();

var app = builder.Build();

// 2) Configure Middleware Pipeline
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// **Quan trọng: Đảm bảo các Middleware liên quan đến Authentication và Authorization ở đúng vị trí**
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseSession();
app.UseAuthentication(); // Xác thực người dùng
app.UseAuthorization(); // Ủy quyền truy cập dựa trên Role/Policy

app.Use(async (context, next) =>
{
    if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
    {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        Console.WriteLine($"📦 Request JSON Body: {body}");
    }

    await next();
});

// Custom Middlewares
app.UseMiddleware<CultureMiddleware>();

// 3) Map Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Product", action = "Search" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();