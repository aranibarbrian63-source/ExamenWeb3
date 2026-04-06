using System.ComponentModel.DataAnnotations;

namespace Examenwed3.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del hotel es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar un precio")]
        [Range(1, 5000, ErrorMessage = "El precio debe estar entre 1 y 5000")]
        public decimal PrecioPorNoche { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        // Relación: Un hotel puede tener muchas reservas
        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}