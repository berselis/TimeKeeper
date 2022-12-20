namespace TimeKeeper.DTOs
{
    public class DailyAssistDTO
    {
        public string Empleado { get; set; }
        public int IdEmpleado { get; set; }
        public bool HasImg { get; set; }
        public List<string> Horas { get; set; }
        public int CantRegistros { get; set; }
        public string TotalHora { get; set; }
    }
}
