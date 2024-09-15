using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeKeeper.DTOs;
using TimeKeeper.Funtions;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    public class DataPrintController : Controller
    {
        private readonly TimerKeeperDbContext context;

        public DataPrintController(TimerKeeperDbContext context)
        {
            this.context = context;
        }



        [HttpPost]
        public async Task<IActionResult> PrintHoraTotal(DateTime FechaIni, DateTime FechaFin, int IdCentro)
        {
            DateTime end = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<Empleado> empleados = await context.Empleados
                   .AsNoTracking()
                   .Include(x => x.Centro)
                   .Where(whe => whe.Estado.Equals("ACTIVO"))
                   .ToListAsync();

            if (IdCentro != 0)
                empleados = empleados.Where(x => x.Centro.IdCentro == IdCentro).ToList();

            List<Tiempo> tiempos = context.Tiempos.Where(whe => FechaIni <= whe.DateReg && whe.DateReg <= end).ToList();

            List<TotalHourDTO> query = TimerProcess.GetTotalHora(empleados, tiempos);

            ViewBag.FechaIni = FechaIni.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = FechaFin.ToString("yyyy-MM-dd");

            return View(query);
        }
    }
}
