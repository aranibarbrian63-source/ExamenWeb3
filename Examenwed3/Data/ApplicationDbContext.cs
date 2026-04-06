using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Examenwed3.Models; // Asegúrate de que el namespace coincida con tu proyecto

namespace Examenwed3.Data
{
    // Heredamos de IdentityDbContext usando nuestra clase de usuario extendida
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas principales del sistema de reservas
        public DbSet<Hotel> Hoteles { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de la relación: Un Hotel tiene muchas Reservas
            builder.Entity<Reserva>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la relación: Un Usuario tiene muchas Reservas
            builder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}