using CleaningApp.Components;
using CleaningApp.Data;
using CleaningApp.Data.Services;
using CleaningApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;


QuestPDF.Settings.License = LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new System.Globalization.CultureInfo("pl-PL");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


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
builder.Services.AddRazorPages();


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<CleaningApp.Data.DbSeeder>();
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
app.UseAntiforgery();


app.MapRazorPages();

// Mapujemy tylko komponenty Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Seed Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdmin.InitializeAsync(services);
    // 2. Us³ugi (Nasz nowy plik)
    var dbSeeder = services.GetRequiredService<CleaningApp.Data.DbSeeder>();
    await dbSeeder.SeedAsync();

}

app.Run();