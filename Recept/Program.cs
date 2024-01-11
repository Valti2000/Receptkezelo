using Recept.Data;
using Microsoft.EntityFrameworkCore;
using Recept.Entity;
using Recept.Services;
using Recept.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Login", "");
});


builder.Services.AddDbContext<ReceptekContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Recept"))
           .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information));

// Identity szolgáltatások hozzáadása
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7045", // Az issuer URL-je
            ValidAudience = "https://localhost:7045", // Az audience URL-je
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EzIttEgyNagyonHosszuTitkosKulcsAmiLegalabb128BitHosszu")) // A titkos kulcs, amin a token alapul
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReceptOlvaso", policy => policy.RequireRole("ReceptOlvaso"));
    // További szerepkör alapú policy-kat is hozzáadhatsz szükség esetén
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Szerepkörök inicializálása
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    IdentityModel.InitializeRoles(serviceProvider).Wait();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{


    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.Run();