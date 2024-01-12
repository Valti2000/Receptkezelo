using Microsoft.AspNetCore.Mvc;
using Recept.Pages.Read; 

namespace Recept.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Alapanyagok()
        {
            return View("/Pages/Read/Alapanyagok.cshtml");
        }

        public IActionResult Allergenek()
        {
            return View("/Pages/Read/Allergenek.cshtml");
        }

        public IActionResult Csoportok()
        {
            return View("/Pages/Read/Csoportok.cshtml");
        }

        public IActionResult Hozzavalok()
        {
            return View("/Pages/Read/Hozzavalok.cshtml");
        }

        public IActionResult Kategoriak()
        {
            return View("/Pages/Read/Kategoriak.cshtml");
        }

        public IActionResult Receptek()
        {
            return View("/Pages/Read/Receptek.cshtml");
        }
    }
}
