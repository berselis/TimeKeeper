using DocumentFormat.OpenXml.Spreadsheet;
using TimeKeeper.Models;

namespace TimeKeeper.Funtions
{
    public static class DatFileProces
    {
        public static List<Tiempo> GetDtoTiempos(string content)
        {
            List<Tiempo> tiempos = new();
            List<string> rows = content.Split("\r\n").SkipLast(1).ToList();

            foreach (string row in rows)
            {
                string[] parts = row.Split("\t");
                string[] datetime = parts[1].Split(' ');

                Tiempo tiempo = new()
                {
                    IdEmpleado = Convert.ToInt32(parts[0]),
                    TimeOut = new(0, 0, 0)
                };

                DateTime date = Convert.ToDateTime($"{datetime[0]} {datetime[1]}");
                TimeSpan time = TimeSpan.Parse(datetime[1]);

                if (!IsValidRangeTime(time))
                {
                    tiempo.HasTimeOutOfRange = true;
                    tiempo.TimeOut = time;
                    date = date.AddDays(-1);
                    time = new(23, 59, 59);
                    date = new(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
                }

                tiempo.DateReg = date;
                tiempo.TimeReg = time;

                tiempos.Add(tiempo);
            }

            return tiempos;
        }
        private static bool IsValidRangeTime(TimeSpan time)
        {
            TimeSpan timeBegin = new(0, 0, 0);
            TimeSpan timeEnd = new(6, 25, 59);
            if (timeBegin < time && time <= timeEnd) return false;
            return true;
        }
    }
}
