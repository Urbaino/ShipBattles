using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace se.Urbaino.ShipBattles.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet("Login/{name}")]
        public async Task<IActionResult> Login(string name)
        {
            var nameClaim = new Claim(ClaimTypes.NameIdentifier, name);
            var identity = new ClaimsIdentity(new[] { nameClaim });
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            return Redirect("/lobby");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
