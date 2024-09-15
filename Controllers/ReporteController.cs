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

        [HttpGet]
        public async Task<IActionResult> DailyAsist()
        {
            List<Centro> centros = await context.Centros
              .AsNoTracking()
              .ToListAsync();

            return View(centros);
        }
        [HttpGet]
        public async Task<IActionResult> HorasRegist()
        {
            List<Centro> centros = await context.Centros
              .AsNoTracking()
              .ToListAsync();

            return View(centros);
        }

        [HttpPost]
        public async Task<JsonResult> FinDataDailyAsist(string dateFirst, int centroId)
        {
            try
            {
                DateTime begin = Convert.ToDateTime(dateFirst);
                DateTime end = begin.AddHours(23).AddMinutes(59).AddSeconds(59);

                List<Empleado> empleados = await context.Empleados
                    .AsNoTracking()
                    .Include(x => x.Centro)
                    .Where(whe => whe.Estado.Equals("ACTIVO"))
                    .ToListAsync();

                if (centroId != 0)
                    empleados = empleados.Where(x => x.Centro.IdCentro == centroId).ToList();

                List<Tiempo> tiempos = await context.Tiempos.Where(whe => begin <= whe.DateReg && whe.DateReg <= end).ToListAsync();

                var response = new
                {
                    status = true,
                    query = TimerProcess.GetDailyAssist(empleados, tiempos)
                };


                return Json(response);

            }
            catch (Exception error)
            {
                var response = new
                {
                    status = false,
                    query = error.Message
                };
                return Json(response);
            }
        }

        [HttpPost]
        public async Task<JsonResult> FinDataTotalHours(DateTime fechaIni, DateTime fechaFin, int centroId)
        {
            try
            {
                DateTime end = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                List<Empleado> empleados = await context.Empleados
                    .AsNoTracking()
                    .Include(x => x.Centro)
                    .Where(whe => whe.Estado.Equals("ACTIVO"))
                    .ToListAsync();

                if (centroId != 0)
                    empleados = empleados.Where(x => x.Centro.IdCentro == centroId).ToList();

                List<Tiempo> tiempos = await context.Tiempos.Where(whe => fechaIni <= whe.DateReg && whe.DateReg <= end).ToListAsync();


                var response = new
                {
                    status = true,
                    query = TimerProcess.GetTotalHora(empleados, tiempos)
                };

                return Json(response);



            }
            catch(Exception error)
            {
                var response = new
                {
                    status = false,
                    query = error.Message
                };
                return Json(response);

            }
            
        }
    }
}