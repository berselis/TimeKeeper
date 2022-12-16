using TimeKeeper.Models;

namespace TimeKeeper.DTOs
{
    public class PanelCargaDTO
    {
        public string FechaActualizado { get; set; }
        public string Usuario { get; set; }
        public string FechaAplicado { get; set; }
        public string Msj { get; set; }
        public ICollection<RegistroCarga> Cargas { get; set; }
    }
}
