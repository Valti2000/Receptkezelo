using Recept.Data;
using Microsoft.EntityFrameworkCore;
using Recept.Entity;
using Recept.Services;
using Recept.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Login", "");
});

builder.Services.AddDbContext<ReceptekContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Recept"))
       .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ReceptekContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IReceptRepository, ReceptRepository>();
builder.Services.AddScoped<UnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAllergenRepository, AllergenRepository>();
builder.Services.AddScoped<IAlapanyagRepository, AlapanyagRepository>();
builder.Services.AddScoped<IKategoriaRepository, KategoriaRepository>();
builder.Services.AddScoped<IHozzavaloRepository, HozzavaloRepository>();
builder.Services.AddScoped<ICsoportRepository, CsoportRepository>();
builder.Services.AddScoped<IReceptHozzavaloRepository, ReceptHozzavaloRepository>();
builder.Services.AddScoped<IAlapanyagAllergenRepository, AlapanyagAllergenRepository>();
builder.Services.AddControllers();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();

    endpoints.MapControllerRoute(
        name: "login",
        pattern: "/login",
        defaults: new { controller = "Home", action = "Login" }
    );

    endpoints.MapControllerRoute(
        name: "receptek",
        pattern: "/Receptek/{action=Index}/{id?}",
        defaults: new { controller = "Home" });
});
app.Run();
