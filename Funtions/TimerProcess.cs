using TimeKeeper.DTOs;
using TimeKeeper.Models;

namespace TimeKeeper.Funtions
{
    public static class TimerProcess
    {

        public static List<DailyAssistDTO> GetDailyAssist(List<Empleado> empleados, List<Tiempo> tiempos)
        {
            List<DailyAssistDTO> result = new();
            foreach (Empleado empleado in empleados)
            {
                List<Tiempo> timeFilter = tiempos.Where(whe => whe.IdEmpleado == empleado.IdEmpleado).ToList();
                TimeSpan totalHours = new(0, 0, 0);

                TimeSpan[] horas = timeFilter.Select(sel => sel.TimeReg).ToArray();
                int size = horas.Length;
                switch (size)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                    case 3:
                        totalHours = horas[1] - horas[0];
                        break;
                    case 4:
                        totalHours = (horas[1] - horas[0]) + (horas[3] - horas[2]);
                        break;
                }



                DailyAssistDTO dto = new()
                {
                    Empleado = empleado.Nombre,
                    IdEmpleado = empleado.IdEmpleado,
                    HasImg = empleado.HasImg,
                    Horas = timeFilter.Select(sel => sel.DateReg.ToString("hh:mm tt")).ToList(),
                    CantRegistros = timeFilter.Count(),
                    TotalHora = $"{totalHours.Days * 24 + totalHours.Hours}h y {totalHours.Minutes}m"
                };

                if (dto.CantRegistros > 0) result.Add(dto);
            }
            result = result.OrderBy(ord => ord.Empleado).ToList();
            return result;
        }
    }
}
