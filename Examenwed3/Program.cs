using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Examenwed3.Data;
using Examenwed3.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE LA CONEXIÓN (Usa solo una cadena de conexión)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. REGISTRO DEL CONTEXTO ÚNICO
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. CONFIGURACIÓN DE IDENTITY (Con soporte para Roles y ApplicationUser)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false; // Más fácil para probar en el examen
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>() // ¡Crítico para el Administrador!
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. CREACIÓN AUTOMÁTICA DE ROLES (Opcional pero recomendado para el examen)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "Cliente" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
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

app.UseAuthentication(); // Quién eres
app.UseAuthorization();  // Qué puedes hacer

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Necesario para Identity

app.Run();