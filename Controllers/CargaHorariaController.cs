using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class CargaHorariaController : Controller
    {
        
        public IActionResult PanelCarga() => View();
    }
}
