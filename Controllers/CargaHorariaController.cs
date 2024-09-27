using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeKeeper.DTOs;
using TimeKeeper.Funtions;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class CargaHorariaController : Controller
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly TimerKeeperDbContext context;
        private const int SPMCentral = 1;
        private const int BocaChica = 2;
        private const int LaRomana = 3;
        private const int PlayaNuevaR = 4;

        public CargaHorariaController(IWebHostEnvironment webHostEnvironment, TimerKeeperDbContext context)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region SPM

        [HttpGet]
        public async Task<IActionResult> PanelCargaSanPedroCentral(string msj)
        {
            PanelCargaDTO panelCarga = new()
            {
                Msj = msj,
                Usuario = "N/A",
                FechaAplicado = "N/A",
                FechaActualizado = "N/A",
                Cargas = new List<RegistroCarga>()
            };

            if (await context.RegistrosCargas.AnyAsync())
            {
                List<RegistroCarga> registroCargas = await context.RegistrosCargas
                    .AsNoTracking()
                    .OrderByDescending(ord => ord.FechaAplicado)
                    .Where(x => x.IdCentro == SPMCentral)
                    .ToListAsync();

                if (registroCargas.Any())
                {
                    RegistroCarga last = registroCargas.First();
                    panelCarga.Usuario = last.NombreUsuario;
                    panelCarga.FechaAplicado = last.FechaAplicado.ToString("yyyy-MM-dd hh:mm tt");
                    panelCarga.FechaActualizado = last.FechaRegistro.ToString("yyyy-MM-dd hh:mm tt");
                    panelCarga.Cargas = registroCargas;
                }
            }

            return View(panelCarga);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDataFileSanPedroCentral(IFormFile fileForm)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                string[] splitName = fileForm.FileName.Split('.');
                string format = splitName[splitName.Length - 1];
                if (format != "dat") return RedirectToAction(nameof(PanelCargaSanPedroCentral), new { msj = "invalidFormat" });

                DateTime today = DateTime.Now;
                string prefix = $"{today.Year}{today.Month}{today.Day}{today.Hour}{today.Minute}{today.Second}_";
                string root = Path.Combine(webHostEnvironment.WebRootPath, "media", "files");
                string fileRoot = Path.Combine(root, $"{prefix}{fileForm.FileName}");
                Stream stream = new FileStream(fileRoot, FileMode.Create);
                await fileForm.CopyToAsync(stream);
                stream.Close();

                StreamReader reader = new(fileRoot);
                string content = reader.ReadToEnd();
                reader.Close();


                List<Tiempo> timeRegisters = DatFileProces.GetDTOTiempoText(content);
                if (timeRegisters.Count <= 0)
                {
                    transaction.Rollback();
                    return RedirectToAction(nameof(PanelCargaSanPedroCentral), new { msj = "notTimeRegister" });
                }


                int totalReg = timeRegisters.Count;
                int added = 0;
                foreach (var item in timeRegisters)
                {
                    if (!await context.Tiempos.AnyAsync(any => any.IdEmpleado == item.IdEmpleado && any.DateReg == item.DateReg && any.TimeReg == item.TimeReg))
                    {
                        context.Tiempos.Add(item);
                        added++;
                    }
                }

                RegistroCarga reg = new()
                {
                    NombreUsuario = User.Identity.Name,
                    FechaRegistro = timeRegisters.Last().DateReg,
                    FechaAplicado = DateTime.Now,
                    Comentario = $"{added} de {totalReg} Reg",
                    IdCentro = SPMCentral
                };

                context.RegistrosCargas.Add(reg);
                await context.SaveChangesAsync();

                transaction.Commit();
                return RedirectToAction(nameof(PanelCargaSanPedroCentral), new { msj = "success" });

            }
            catch
            {
                transaction.Rollback();
                return RedirectToAction(nameof(PanelCargaSanPedroCentral), new { msj = "error" });
            }
        }

        #endregion

        #region BOCA CHICA
        [HttpGet]
        public async Task<IActionResult> PanelCargaBocaChica(string msj)
        {

            PanelCargaDTO panelCarga = new()
            {
                Msj = msj,
                Usuario = "N/A",
                FechaAplicado = "N/A",
                FechaActualizado = "N/A",
                Cargas = new List<RegistroCarga>()
            };

            if (await context.RegistrosCargas.AnyAsync())
            {
                List<RegistroCarga> registroCargas = await context.RegistrosCargas
                    .AsNoTracking()
                    .OrderByDescending(ord => ord.FechaAplicado)
                    .Where(x => x.IdCentro == BocaChica)
                    .ToListAsync();

                if (registroCargas.Any())
                {
                    RegistroCarga last = registroCargas.First();

                    panelCarga.Usuario = last.NombreUsuario;
                    panelCarga.FechaAplicado = last.FechaAplicado.ToString("yyyy-MM-dd hh:mm tt");
                    panelCarga.FechaActualizado = last.FechaRegistro.ToString("yyyy-MM-dd hh:mm tt");
                    panelCarga.Cargas = registroCargas;
                }
            }


            return View(panelCarga);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDataFileBocaChica(IFormFile file)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {

                if (file is null || file.Length == 0)
                    return RedirectToAction(nameof(PanelCargaBocaChica), new { msj = "invalidFormat" });

                if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction(nameof(PanelCargaBocaChica), new { msj = "invalidFormat" });

                Stream stream = file.OpenReadStream();
                StreamReader reader = new(stream);

                string content = await reader.ReadToEndAsync();
                List<Tiempo> timeRegisters = DatFileProces.GetDTOTiempoCsv(content);

                if (timeRegisters.Count <= 0)
                {
                    transaction.Rollback();
                    return RedirectToAction(nameof(PanelCargaBocaChica), new { msj = "notTimeRegister" });
                }


                int totalReg = timeRegisters.Count;
                int added = 0;
                foreach (var item in timeRegisters)
                {
                    if (!await context.Tiempos.AnyAsync(dbT => dbT.IdEmpleado == item.IdEmpleado && dbT.DateReg == item.DateReg && dbT.TimeReg == item.TimeReg))
                    {
                        context.Tiempos.Add(item);
                        added++;
                    }
                }

                RegistroCarga reg = new()
                {
                    NombreUsuario = User.Identity.Name,
                    FechaRegistro = timeRegisters.Last().DateReg,
                    FechaAplicado = DateTime.Now,
                    Comentario = $"{added} de {totalReg} Reg",
                    IdCentro = BocaChica
                };

                context.RegistrosCargas.Add(reg);
                await context.SaveChangesAsync();

                transaction.Commit();
                return RedirectToAction(nameof(PanelCargaBocaChica), new { msj = "success" });

            }
            catch
            {
                transaction.Rollback();
                return RedirectToAction(nameof(PanelCargaBocaChica), new { msj = "error" });
            }
        }
        #endregion


        #region LA ROMANA
        [HttpGet]
        public async Task<IActionResult> PanelCargaLaRomana(string msj)
        {
            PanelCargaDTO panelCarga = new()
            {
                Msj = msj,
                Usuario = "N/A",
                FechaAplicado = "N/A",
                FechaActualizado = "N/A",
                Cargas = new List<RegistroCarga>()
            };

            if (await context.RegistrosCargas.AnyAsync())
            {
                List<RegistroCarga> registroCargas = await context.RegistrosCargas
                    .AsNoTracking()
                    .OrderByDescending(ord => ord.FechaAplicado)
                    .Where(x => x.IdCentro == LaRomana)
                    .ToListAsync();

                if (registroCargas.Any())
                {
                    RegistroCarga last = registroCargas.First();

                    panelCarga.Usuario = last.NombreUsuario;
                    panelCarga.FechaAplicado = last.FechaAplicado.ToString("yyyy-MM-dd hh:mm tt");
                    panelCarga.FechaActualizado = last.FechaRegistro.ToString("yyyy-MM-dd hh:mm tt");
                    panelCarga.Cargas = registroCargas;
                }
            }


            return View(panelCarga);

        }
        [HttpPost]
        public async Task<IActionResult> UploadDataFileLaRomana(IFormFile file)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {

                if (file is null || file.Length == 0)
                    return RedirectToAction(nameof(PanelCargaLaRomana), new { msj = "invalidFormat" });

                if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction(nameof(PanelCargaLaRomana), new { msj = "invalidFormat" });

                Stream stream = file.OpenReadStream();
                StreamReader reader = new(stream);

                string content = await reader.ReadToEndAsync();
                List<Tiempo> timeRegisters = DatFileProces.GetDTOTiempoCsv(content);

                if (timeRegisters.Count <= 0)
                {
                    transaction.Rollback();
                    return RedirectToAction(nameof(PanelCargaLaRomana), new { msj = "notTimeRegister" });
                }


                int totalReg = timeRegisters.Count;
                int added = 0;
                foreach (var item in timeRegisters)
                {
                    if (!await context.Tiempos.AnyAsync(dbT => dbT.IdEmpleado == item.IdEmpleado && dbT.DateReg == item.DateReg && dbT.TimeReg == item.TimeReg))
                    {
                        context.Tiempos.Add(item);
                        added++;
                    }
                }

                RegistroCarga reg = new()
                {
                    NombreUsuario = User.Identity.Name,
                    FechaRegistro = timeRegisters.Last().DateReg,
                    FechaAplicado = DateTime.Now,
                    Comentario = $"{added} de {totalReg} Reg",
                    IdCentro = LaRomana
                };

                context.RegistrosCargas.Add(reg);
                await context.SaveChangesAsync();

                transaction.Commit();
                return RedirectToAction(nameof(PanelCargaLaRomana), new { msj = "success" });

            }
            catch
            {
                transaction.Rollback();
                return RedirectToAction(nameof(PanelCargaLaRomana), new { msj = "error" });
            }
        }




        #endregion

        #region FUNCTION

        #endregion

    }
}
