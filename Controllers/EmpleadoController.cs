using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class EmpleadoController : Controller
    {
        private readonly TimeKeeperDBContext DB;
        public EmpleadoController(TimeKeeperDBContext context)
        {
            DB = context;
        }


        public async Task<IActionResult> Index(string msj)
        {
            List<Empleados> empleados = await DB.Empleados.ToListAsync();
            ViewBag.Msj = msj;
            return View(empleados);
        }

        public async Task<IActionResult> Registrar(Empleados model)
        {
            
            DB.Empleados.Add(model);
            await DB.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { msj = "added" });
        }


        [HttpGet("id:int")]
        public async Task<IActionResult> Empleado(int Id)
        {
            Empleados empleado = await DB.Empleados.FindAsync(Id);
            return View(empleado);
        }
        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(Empleados model)
        {
            Empleados empleado = await DB.Empleados.FindAsync(model.IdEmpleado);
            empleado.Nombre = model.Nombre;
            empleado.Departamento = model.Departamento;
            await DB.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {msj = "edited"});
        }
    }
}
