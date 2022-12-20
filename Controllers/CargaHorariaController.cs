using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using TimeKeeper.DTOs;
using TimeKeeper.Funtions;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class CargaHorariaController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly TimerKeeperDbContext _context;

        public CargaHorariaController(IWebHostEnvironment webHostEnvironment, TimerKeeperDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> PanelCarga(string msj)
        {

            PanelCargaDTO panelCarga = new()
            {
                Msj = msj,
                Usuario = "N/A",
                FechaAplicado = "N/A",
                FechaActualizado = "N/A",
                Cargas = new List<RegistroCarga>()
            };

            if(await _context.RegistrosCargas.AnyAsync())
            {
                List<RegistroCarga> registroCargas = await _context.RegistrosCargas.ToListAsync();
                RegistroCarga last = registroCargas.Last();

                panelCarga.Usuario = last.NombreUsuario;
                panelCarga.FechaAplicado = last.FechaAplicado.ToString("yyyy-MM-dd hh:mm tt");
                panelCarga.FechaActualizado = last.FechaRegistro.ToString("yyyy-MM-dd hh:mm tt");

                panelCarga.Cargas = registroCargas;

            }
           
            return View(panelCarga);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDataFile(IFormFile fileForm)
        {
            try
            {
                string[] splitName = fileForm.FileName.Split('.');
                string format = splitName[splitName.Length - 1];
                if (format != "dat") return RedirectToAction(nameof(PanelCarga), new { msj = "invalidFormat" });
                DateTime today = DateTime.Now;
                string prefix = $"{today.Year}{today.Month}{today.Day}{today.Hour}{today.Minute}{today.Second}_";
                string root = Path.Combine(_webHostEnvironment.WebRootPath, "media", "files");
                string fileRoot = Path.Combine(root, $"{prefix}{fileForm.FileName}");
                Stream stream = new FileStream(fileRoot, FileMode.Create);
                await fileForm.CopyToAsync(stream);
                stream.Close();


                StreamReader reader = new(fileRoot);
                string content = reader.ReadToEnd();
                reader.Close();


                List<Tiempo> timeRegisters = DatFileProces.GetDtoTiempos(content);
                if (timeRegisters.Count <= 0) return RedirectToAction(nameof(PanelCarga), new { msj = "notTimeRegister" });


                int totalReg = timeRegisters.Count;

                if (_context.RegistrosCargas.Any())
                {
                    List<RegistroCarga> registroCargas = await _context.RegistrosCargas.ToListAsync();
                    RegistroCarga last = registroCargas.Last();
                    timeRegisters = timeRegisters.Where(whe => whe.DateReg > last.FechaRegistro).ToList();
                }

                int filterReg = timeRegisters.Count;

                if(filterReg <= 0) return RedirectToAction(nameof(PanelCarga), new { msj = "duplicated" });

                _context.Tiempos.AddRange(timeRegisters);


                RegistroCarga reg = new()
                {
                    NombreUsuario = User.Identity.Name,
                    FechaRegistro = timeRegisters.Last().DateReg,
                    FechaAplicado = DateTime.Now,
                    Comentario = $"{totalReg} de {filterReg} Reg"
                };

                _context.RegistrosCargas.Add(reg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PanelCarga), new { msj = "success" });

            }
            catch
            {
                return RedirectToAction(nameof(PanelCarga), new { msj = "error" });
            }


        }
    }
}
