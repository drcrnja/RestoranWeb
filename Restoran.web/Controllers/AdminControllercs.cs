using Microsoft.AspNetCore.Mvc;

namespace Restoran.web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                HttpContext.Session.SetString("role", "admin");
                return RedirectToAction("UnosRezervacija", "Admin");
            }

            ViewBag.Error = "Pogrešan username ili password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("role");
            return RedirectToAction("Login");
        }

        public IActionResult UnosRezervacija()
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login");

            return RedirectToAction("Create", "Rezervacija");
        }

        public IActionResult UpravljanjeRezervacijama()
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Login");

            return RedirectToAction("Index", "Rezervacija");
        }
    }
}
