using CleaningApp.Components;
using CleaningApp.Data;
using CleaningApp.Data.Services;
using CleaningApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- POPRAWNA KONFIGURACJA IDENTITY DLA BLAZOR + RAZOR PAGES ---
// (Ten blok zastępuje stary AddDefaultIdentity)
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();
// --- KONIEC BLOKU IDENTITY ---

// --- DODAJEMY OBSŁUGĘ RAZOR PAGES ---
// (Niezbędne do stron .cshtml w /Areas/Identity/)
builder.Services.AddRazorPages();

// --- Usługi Aplikacji ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ServiceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery(); // Poprawna kolejność

// --- MAPUJEMY RAZOR PAGES ---
// (Niezbędne do stron .cshtml w /Areas/Identity/)
app.MapRazorPages();

// Mapujemy tylko komponenty Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Seed Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdmin.InitializeAsync(services);
}

app.Run();