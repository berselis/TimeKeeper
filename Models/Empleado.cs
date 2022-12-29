

using System.ComponentModel.DataAnnotations;

namespace TimeKeeper.Models
{
    public class Empleado
    {

        [Key]
        public int IdEmpleado { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [Required, MaxLength(100)]
        public string Departamento { get; set; }
        public bool HasImg { get; set; }
        [Required, MaxLength(10)]
        public string Estado { get; set; } = "ACTIVO";
        public ICollection<Tiempo> Tiempos { get; set; }
    }
}