using Microsoft.AspNetCore.Mvc;
using Examenwed3.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Examenwed3.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // CORRECCIÓN CRÍTICA: En tu modelo la clase es 'Hotel'. 
                // Entity Framework suele pluralizarlo como 'Hotels' en el DbContext.
                ViewBag.TotalHoteles = _context.Hoteles.Count();

                ViewBag.TotalReservas = _context.Reservas.Count();

                // Para los usuarios de Identity
                ViewBag.TotalUsuarios = _context.Users.Count();
            }
            catch (Exception ex)
            {
                // PLAN DE EMERGENCIA: Si la base de datos falla, 
                // enviamos ceros para que la interfaz cargue de todos modos.
                ViewBag.TotalHoteles = 0;
                ViewBag.TotalReservas = 0;
                ViewBag.TotalUsuarios = 0;

                // Esto imprimirá el error real en la consola de salida de Visual Studio
                System.Diagnostics.Debug.WriteLine("Error en Dashboard: " + ex.Message);
            }

            return View();
        }
    }
}