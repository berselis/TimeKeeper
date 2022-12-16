using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class UsuarioController : Controller
    {
        private readonly TimerKeeperDbContext _context;
        private readonly UserManager<Usuario> userManager;

        public UsuarioController(TimerKeeperDbContext DB, UserManager<Usuario> userManager)
        {
            this._context = DB;
            this.userManager = userManager;
        }

        public async Task<ActionResult> Index(string msj)
        {
            List<Usuario> usuario = await _context.Usuarios.Select(sel => new Usuario
            {
                Id = sel.Id,
                UserName = sel.UserName,
                RoleName = sel.RoleName,

            }).ToListAsync();
            ViewBag.Msj = msj;
            return View(usuario);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Usuario(Usuario model)
        {
            Usuario usuario = await _context.Usuarios.FindAsync(model.Id);


            return RedirectToAction("Index", new { msj = "editado" });
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UserCredential model)
        {
            Usuario usuario = new()
            {
                UserName = model.UserName,
                RoleName = model.RoleName ?? "Agente",
            };


            IdentityResult result = await userManager.CreateAsync(usuario, model.Password);

            if (result.Succeeded) return RedirectToAction("Index", new { msj = "creado" });


            return RedirectToAction("Index", new { msj = result.Errors });
        }


    }
}
