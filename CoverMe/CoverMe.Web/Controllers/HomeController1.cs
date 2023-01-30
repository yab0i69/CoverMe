using Microsoft.AspNetCore.Mvc;

namespace CoverMe.Web.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
