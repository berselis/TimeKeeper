using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeKeeper.DTOs.Employ;
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

        [HttpGet]
        public async Task<IActionResult> Index(string msj)
        {
            List<Empleado> empleados = await _context.Empleados
                .AsNoTracking()
                .Include(em => em.Centro)
                .OrderBy(ord => ord.Nombre)
                .ToListAsync();


            List<Centro> centros = await _context.Centros
                .AsNoTracking()
                .ToListAsync();


            ViewBag.Msj = msj;
            ViewBag.Sucursal = 0;


            EmpleadoViewListModelDTO viewModel = new()
            {
                Empleados = empleados,
                Centros = centros
            };



            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int IdCentro)
        {

            List<Empleado> empleados = await _context.Empleados
               .AsNoTracking()
               .Include(em => em.Centro)
               .OrderBy(ord => ord.Nombre)
               .ToListAsync();


            List<Centro> centros = await _context.Centros
                .AsNoTracking()
                .ToListAsync();

            if (IdCentro != 0)
            {
                empleados = empleados.Where(x => x.Centro.IdCentro == IdCentro).ToList();
            }


            ViewBag.Sucursal = IdCentro;


            EmpleadoViewListModelDTO viewModel = new()
            {
                Empleados = empleados,
                Centros = centros
            };



            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Empleado model)
        {
            try
            {
                if (await _context.Empleados.AnyAsync(x => x.IdEmpleado == model.IdEmpleado))
                    return RedirectToAction(nameof(Index), new { msj = "duplicated" });

                _context.Empleados.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { msj = "added" });

            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { msj = "error" });
            }


        }


        [HttpGet("id:int")]
        public async Task<IActionResult> Empleado(int Id)
        {
            Empleado empleado = await _context.Empleados
                .Include(x => x.Centro)
                .FirstOrDefaultAsync(x => x.IdEmpleado == Id);

            List<Centro> centros = await _context.Centros
              .AsNoTracking()
              .Where(x => x.IdCentro != empleado.Centro.IdCentro)
              .ToListAsync();

            EmpleadoViewModelDTO viewModel = new()
            {
                Empleado = empleado,
                Centros = centros

            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(Empleado model)
        {
            Empleado empleado = await _context.Empleados.FindAsync(model.IdEmpleado);

            empleado.Estado = string.IsNullOrEmpty(model.Estado) ? "N/A" : "ACTIVO";
            empleado.IdCentro = model.IdCentro;
            empleado.Nombre = model.Nombre;
            empleado.Departamento = model.Departamento;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { msj = "edited" });
        }
    }
}
