using Microsoft.AspNetCore.Mvc;

namespace CoverMe.Web.Controllers
{
    public class CalandarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
