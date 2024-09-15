
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Models
{
    public class Tiempo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int IdTime { get; set; }
        [Required]
        public int IdEmpleado { get; set; }
        [Required]
        public DateTime DateReg { get; set; }
        [Required]
        public TimeSpan TimeReg { get; set; }
        [Required]
        public bool HasTimeOutOfRange { get; set; }
        [Required]
        public TimeSpan TimeOut { get; set; }

        [ForeignKey("IdEmpleado"), InverseProperty("Tiempos")]
        public virtual Empleado Empleado { get; set; }
    }
}