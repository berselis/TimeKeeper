using DocumentFormat.OpenXml.InkML;
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
        public ActionResult PrintHoraTotal(DateTime FechaIni, DateTime FechaFin)
        {
            DateTime end = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<Empleado> empleados = context.Empleados.Where(whe => whe.Estado.Equals("ACTIVO")).ToList();
            List<Tiempo> tiempos = context.Tiempos.Where(whe => FechaIni <= whe.DateReg && whe.DateReg <= end).ToList();

            List<TotalHourDTO> query = TimerProcess.GetTotalHora(empleados, tiempos);

            ViewBag.FechaIni = FechaIni.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = FechaFin.ToString("yyyy-MM-dd");

            return View(query);
        }
    }
}
