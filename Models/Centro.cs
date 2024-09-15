using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Models
{
    public class Centro
    {
        public Centro()
        {
            Empleados = new HashSet<Empleado>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int IdCentro { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [InverseProperty("Centro")]
        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}