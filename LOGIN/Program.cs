using Microsoft.EntityFrameworkCore;
using LOGIN.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Creamos la ruta de forma compatible con Linux de Render
string dbPath = Path.Combine("/tmp", "techstore.db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 2. CREACIÓN AUTOMÁTICA DE LA BASE DE DATOS LOCAL
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        Console.WriteLine("¡Base de datos SQLite creada con éxito en /tmp/techstore.db!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error critico al crear la base de datos: {ex.Message}");
    }
}

// 3. PIPELINE DE LA APLICACIÓN
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers();

app.Run();