using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Examenwed3.Models
{
    // Hereda de IdentityUser para mantener las funciones de login/password
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}