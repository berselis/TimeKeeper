using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Models
{
    public class RegistroCarga
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int IdRegistroCarga { get; set; }
        [Required, MaxLength(100)]
        public string NombreUsuario { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        [Required]
        public DateTime FechaAplicado { get; set; }
        [Required, MaxLength(100)]
        public string Comentario { get; set; }

        [Required]
        public int IdCentro { get; set; }
    }
}
