using TimeKeeper.Models;

namespace TimeKeeper.DTOs.Employ
{
    public class EmpleadoViewModelDTO
    {
        public IEnumerable<Centro> Centros { get; set; }
        public Empleado Empleado { get; set; }
    }
}
