

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Models
{
    public class Empleado
    {
        public Empleado()
        {
            Tiempos = new HashSet<Tiempo>();
        }

        [Key]
        public int IdEmpleado { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [Required, MaxLength(100)]
        public string Departamento { get; set; }
        public bool HasImg { get; set; }
        [Required, MaxLength(10)]
        public string Estado { get; set; }
        public int IdCentro { get; set; }

        [ForeignKey("IdCentro"), InverseProperty("Empleados")]
        public virtual Centro Centro { get; set; }

        [InverseProperty("Empleado")]
        public virtual ICollection<Tiempo> Tiempos { get; set; }
    }
}