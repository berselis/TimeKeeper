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
                    DateReg = Convert.ToDateTime($"{datetime[0]} {datetime[1]}"),
                    TimeReg = TimeSpan.Parse(datetime[1])

                };

                tiempos.Add(tiempo);
            }

            return tiempos;
        }
    }
}
