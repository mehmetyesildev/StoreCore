using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddDbContext<StoreDbContext>(Options =>
{
    Options.UseSqlite(builder.Configuration["ConnectionStrings:StoreDbConnection"], b => b.MigrationsAssembly("StoreApp.Web"));
});
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<StoreDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(Options =>
{
    Options.Password.RequiredLength = 6;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequireLowercase = false;
    Options.Password.RequireUppercase = false;
    Options.Password.RequireDigit = false;
    Options.User.RequireUniqueEmail = true;
    Options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    Options.Lockout.MaxFailedAccessAttempts = 5;
    Options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.ConfigureApplicationCookie(Options =>
{
    Options.LoginPath = "/Account/Login";
    Options.AccessDeniedPath = "/Account/AccessDenied";
    Options.SlidingExpiration = true;
    Options.ExpireTimeSpan = TimeSpan.FromDays(30);
});


builder.Services.AddScoped<IStoreRepository,EFStoreRepostory>();
builder.Services.AddScoped<IOrderRepository,EFOrderRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddScoped<Cart>(sc=>SessionCard.GetCart(sc));

var app = builder.Build();
app.UseSession();
app.UseStaticFiles();

app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);
//product/telefon=>kategori urun listesi
app.MapControllerRoute("product_in_catrgory", "products/{category?}", new { controller = "Home", action = "Index" });
//samsun-24=>urundetay
app.MapControllerRoute("product_details","{name}",new{controller="Home", action="Details"});
app.MapDefaultControllerRoute();

app.Run();
