using Recept.Data;
using Microsoft.EntityFrameworkCore;
using Recept.Entity;
using Recept.Services;
using Recept.Repositories;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ReceptekContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Recept")));
builder.Services.AddScoped<IReceptRepository, ReceptRepository>();
builder.Services.AddScoped<UnitOfWork, UnitOfWork>(); // Hozzáadva a DI konténerhez
builder.Services.AddScoped<IAllergenRepository, AllergenRepository>();
builder.Services.AddScoped<IAlapanyagRepository, AlapanyagRepository>();
builder.Services.AddScoped<IKategoriaRepository, KategoriaRepository>();
builder.Services.AddScoped<IHozzavaloRepository, HozzavaloRepository>();
builder.Services.AddScoped<ICsoportRepository, CsoportRepository>();
builder.Services.AddScoped<IReceptHozzavaloRepository, ReceptHozzavaloRepository>();
builder.Services.AddScoped<IAlapanyagAllergenRepository, AlapanyagAllergenRepository>();
builder.Services.AddControllers();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Ezt hozzáadva kezelni fogja a vezérlőd végpontjait
    endpoints.MapRazorPages();

    // Itt más végpontokat is hozzáadhatsz, ha szükséges
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");
});


app.Run();

