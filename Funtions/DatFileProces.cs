using TimeKeeper.Models;

namespace TimeKeeper.Funtions
{
    public static class DatFileProces
    {
        public static List<Tiempo> GetDTOTiempoText(string content)
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

            tiempos = tiempos.OrderBy(ord => ord.DateReg).ToList();

            return tiempos;
        }
        public static List<Tiempo> GetDTOTiempoCsv(string content)
        {
            List<Tiempo> tiempos = new();
            List<string> rows = content.Split("\r\n").Skip(1).SkipLast(1).ToList();

            foreach (string columns in rows)
            {

                string[] column = columns.Split(',');

                Tiempo tiempo = new()
                {
                    IdEmpleado = Convert.ToInt32(column[2]),
                    TimeOut = new(0, 0, 0)
                };

                DateTime date = Convert.ToDateTime($"{column[5]} {column[6]}");
                TimeSpan time = date.TimeOfDay;

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
            tiempos = tiempos.OrderBy(ord => ord.DateReg).ToList();

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
