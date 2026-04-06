using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Examenwed3.Data;
using Examenwed3.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE LA CONEXIÓN CON REINTENTOS (Evita el error de falla transitoria)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(); // Sugerido por el error en tu captura
    }));

// 2. CONFIGURACIÓN DE IDENTITY
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 3. CREACIÓN SEGURA DE ROLES (Corregido para evitar el bloqueo del programa)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "Admin", "Cliente" };

        foreach (var roleName in roleNames)
        {
            // Usamos .GetAwaiter().GetResult() para que el Main espere sin fallar
            var roleExist = roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult();
            if (!roleExist)
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
        }
    }
    catch (Exception ex)
    {
        // Si la DB no está lista, el programa sigue adelante en lugar de cerrarse
        Console.WriteLine("Log: No se pudieron crear los roles aún. " + ex.Message);
    }
}

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"); // Cambiado a Dashboard como inicio

app.MapRazorPages();

app.Run();