using DocumentFormat.OpenXml.Presentation;

namespace TimeKeeper.DTOs
{
    public class DailyAssistDTO
    {
        public string Empleado { get; set; }
        public string Sucursal { get; set; }
        public int IdEmpleado { get; set; }
        public bool HasImg { get; set; }
        public List<Hora> Horas { get; set; }
        public int CantRegistros { get; set; }
        public bool HasTimeOutOfRange { get; set; }
        public string TimeOutOfRange { get; set; }
        public string TotalHora { get; set; }
    }

    public class Hora
    {
        public string HoraReg { get; set; }
        public bool HasTimeOutOfRange { get; set; }
        public string TimeOutOfRange { get; set; }
    }
}
