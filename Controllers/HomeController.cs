using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TimeKeeper.Funtions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TimeKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Usuario> signInManager;
        private readonly UserManager<Usuario> userManager;
        private readonly TimerKeeperDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            SignInManager<Usuario> signInManager,
            UserManager<Usuario> userManager,
            TimerKeeperDbContext context)
        {
            _logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index(string msj)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated) return RedirectToAction(nameof(Panel));

            ViewBag.Msj = msj;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(UserCredential modelIn)
        {
            var logIn = await signInManager.PasswordSignInAsync(modelIn.UserName, modelIn.Password, true, false);
            if (logIn.Succeeded == false) return RedirectToAction(nameof(Index), new { msj = "invalid" });
            Usuario userLog = await userManager.FindByNameAsync(modelIn.UserName);
            List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, userLog.UserName),
                    new Claim("Role", userLog.RoleName)
                };
            await userManager.AddClaimAsync(userLog, new Claim("Role", userLog.RoleName));
            return RedirectToAction(nameof(Panel));
        }
        public async Task<IActionResult> LogOut()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Panel()
        {
            return View();
        }


    }
}