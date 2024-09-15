using TimeKeeper.Models;

namespace TimeKeeper.DTOs.Employ
{
    public class EmpleadoViewListModelDTO
    {
        public IEnumerable<Empleado> Empleados { get; set; }
        public IEnumerable<Centro> Centros { get; set; }
    }
}
