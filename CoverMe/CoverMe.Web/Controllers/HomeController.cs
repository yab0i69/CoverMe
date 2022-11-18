using CoverMe.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoverMe.Web.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController<HomeController>
{
    public IActionResult Index(bool? register)
    {
        HomePageModel vm = new HomePageModel { 
            NewRegister = register ?? false
        };

        return View(vm);
    }

    // Template actions
    public IActionResult About() => View();
    public IActionResult TOS() => View();
    public IActionResult Privacy() => View();
}