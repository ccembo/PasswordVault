using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PVDB.Data;
using PasswordVault.WebUIServer.Configuration;
using SignalRKeyExchange.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//KeyExchange requirement, Temporal Store
builder.Services.AddSingleton<IKeyRepository, keyRepository>();

//KeyExchange requirement 
builder.Services.AddSignalR();

// Access configuration
var config = builder.Configuration.GetSection("PVServerConfig");
builder.Services.Configure<ServerConfiguration>(config);

//Data
builder.Services.AddDbContext<PVDBContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PVDBContext") ?? throw new InvalidOperationException("Connection string 'PVDB' not found.")));

//Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("PVScheme", options =>
    {
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
    options.SlidingExpiration = true;
    }
    );


// Add authorization, so you can later restrict access by role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedUsers", policy => policy.RequireRole("User,Read-Only User,Admin"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

//Sessions
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseAuthentication();
app.UseAuthorization();

//Sessions
app.UseSession();

app.MapRazorPages();
app.MapHub<KeyExchangeHub>("/KeyExchangeHub");

app.Run();
