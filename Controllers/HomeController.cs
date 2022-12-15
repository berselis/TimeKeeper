using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TimeKeeper.Funtions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Usuario> signInManager;
        private readonly UserManager<Usuario> userManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
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
            var result = await signInManager.PasswordSignInAsync(modelIn.UserName, modelIn.Password, true, false);
            if(result.Succeeded)
            {
                var usuario = await userManager.FindByNameAsync(modelIn.UserName);
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.UserName),
                    new Claim("Role", usuario.RoleName)
                };
                await userManager.AddClaimAsync(usuario, new Claim("Role", usuario.RoleName));
                return RedirectToAction(nameof(Panel));
            }
            return RedirectToAction(nameof(Index), new {msj = "invalid"});
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