using DummyAD1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DummyAD1.Controllers
{
   //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var isAdAuthEnabled = User.Identity is not null && User.Identity.IsAuthenticated &&
                                  (User.Identity.AuthenticationType == "Negotiate" || User.Identity.AuthenticationType == "Kerberos" || User.Identity.AuthenticationType == "NTLM");

            ViewBag.IsAdAuthEnabled = isAdAuthEnabled;

            // Just to check what identity is being passed
            ViewBag.Username = User.Identity?.Name;
            ViewBag.AuthType = User.Identity?.AuthenticationType;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
