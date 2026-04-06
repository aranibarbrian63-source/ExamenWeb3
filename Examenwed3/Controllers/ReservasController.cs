using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examenwed3.Data;
using Examenwed3.Models;
using Microsoft.AspNetCore.Authorization; // Necesario para [Authorize]
using System.Security.Claims; // Necesario para obtener el ID del usuario

namespace Examenwed3.Controllers
{
    [Authorize] // Solo usuarios registrados pueden gestionar reservas
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context; // Usa tu contexto real

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            // Si es administrador ve todo, si es cliente solo ve las suyas
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Reservas
                .Include(r => r.Hotel)
                .Include(r => r.Usuario);

            if (User.IsInRole("Admin"))
            {
                return View(await query.ToListAsync());
            }

            return View(await query.Where(r => r.UsuarioId == userId).ToListAsync());
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            // Cambiamos "Direccion" por "Nombre" para que sea más fácil elegir el hotel
            ViewData["HotelId"] = new SelectList(_context.Hoteles, "Id", "Nombre");
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaInicio,FechaFin,HotelId")] Reserva reserva)
        {
            // 1. Asignar el ID del usuario logueado automáticamente
            reserva.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. VALIDACIÓN: No permitir fechas pasadas
            if (reserva.FechaInicio < DateTime.Now.Date)
            {
                ModelState.AddModelError("FechaInicio", "No se permiten reservas en fechas pasadas.");
            }

            // 3. VALIDACIÓN: Fecha fin debe ser mayor a inicio
            if (reserva.FechaFin <= reserva.FechaInicio)
            {
                ModelState.AddModelError("FechaFin", "La fecha de finalización debe ser posterior a la fecha de inicio.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["HotelId"] = new SelectList(_context.Hoteles, "Id", "Nombre", reserva.HotelId);
            return View(reserva);
        }

        // Los métodos Edit, Details y Delete se pueden mantener similares, 
        // pero recuerda aplicar las mismas validaciones de fecha en el Edit POST.

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}