using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoverMe.Web.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]



    public class pleaseconfirmController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

