using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeKeeper.Funtions;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class ReporteController : Controller
    {
        private readonly TimerKeeperDbContext context;
        public ReporteController(TimerKeeperDbContext context)
        {
            this.context = context;
        }

        public IActionResult PanelReport() => View();
        public IActionResult DailyAsist() => View();
        public IActionResult HorasRegist() => View();

        [HttpPost]
        public async Task<JsonResult> FinDataDailyAsist(string dateFirst)
        {
            try
            {
                DateTime begin = Convert.ToDateTime(dateFirst);
                DateTime end = begin.AddHours(23).AddMinutes(59).AddSeconds(59);

                List<Empleado> empleados = await context.Empleados.Where(whe => whe.Estado.Equals("ACTIVO")).ToListAsync();
                List<Tiempo> tiempos = await context.Tiempos.Where(whe => begin <= whe.DateReg && whe.DateReg <= end).ToListAsync();
                return Json(TimerProcess.GetDailyAssist(empleados, tiempos));

            }
            catch (Exception error)
            {
                return Json(error.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> FinDataTotalHours(DateTime fechaIni, DateTime fechaFin)
        {
            DateTime end = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<Empleado> empleados = await context.Empleados.Where(whe => whe.Estado.Equals("ACTIVO")).ToListAsync();
            List<Tiempo> tiempos = await context.Tiempos.Where(whe => fechaIni <= whe.DateReg && whe.DateReg <= end).ToListAsync();

            return Json(TimerProcess.GetTotalHora(empleados, tiempos));
        }


    }
}
