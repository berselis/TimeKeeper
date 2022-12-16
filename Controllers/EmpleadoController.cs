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
        private readonly TimerKeeperDbContext _context;
        public EmpleadoController(TimerKeeperDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string msj)
        {
            List<Empleado> empleados = await _context.Empleados.ToListAsync();
            ViewBag.Msj = msj;
            return View(empleados);
        }

        public async Task<IActionResult> Registrar(Empleado model)
        {
            
            _context.Empleados.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { msj = "added" });
        }


        [HttpGet("id:int")]
        public async Task<IActionResult> Empleado(int Id)
        {
            Empleado empleado = await _context.Empleados.FindAsync(Id);
            return View(empleado);
        }
        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(Empleado model)
        {
            Empleado empleado = await _context.Empleados.FindAsync(model.IdEmpleado);
            empleado.Nombre = model.Nombre;
            empleado.Departamento = model.Departamento;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {msj = "edited"});
        }
    }
}
