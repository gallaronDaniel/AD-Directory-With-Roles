using DummyAD1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DummyAD1.Controllers
{
    // HomeController
    public class HomeController : Controller
    {
        private static readonly Dictionary<string, List<User>> users = new()
        {
            ["Employee"] = new List<User>
        {
            new User { Name = "Alice Johnson", Email = "alice.johnson@company.com"},
            new User {Name = "Bob Smith", Email = "bob.smith@company.com"},
            new User {Name = "Charlie Brown", Email = "charlie.brown@company.com"}
        },
            ["Admin"] = new List<User>
        {
            new User {Name = "David Lee", Email = "david.lee@company.com"   },
            new User {Name = "Eva Green", Email = "eva.green@company.com"},
            new User {Name = "Frank White", Email = "frank.white@company.com"}
        },
            ["HR"] = new List<User>
        {
            new User {Name = "Grace Miller", Email = "grace.miller@company.com"},
            new User {Name = "Hannah Davis", Email = "hannah.davis@company.com"},
            new User {Name = "Ian Moore", Email = "ian.moore@company.com"}
        }
        };

        public IActionResult Login()
        {
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user)
        {
            var selectedUser = users.SelectMany(u => u.Value).FirstOrDefault(u => u.Name == user);

            if (selectedUser == null)
                return RedirectToAction("Login");

            var role = users.FirstOrDefault(u => u.Value.Contains(selectedUser)).Key;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, selectedUser.Name),
            new Claim(ClaimTypes.Email, selectedUser.Email),
            new Claim(ClaimTypes.Role, role)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.UserName = User.Identity?.Name;
            ViewBag.UserEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (User.IsInRole("Admin"))
                return RedirectToAction("AdminHome");
            else if (User.IsInRole("HR"))
                return RedirectToAction("HRHome");
            else
                return RedirectToAction("EmployeeHome");
        }


        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminHome() => View("~/Views/Admin/AdminHome.cshtml");

        [Authorize(Policy = "HROnly")]
        public IActionResult HRHome() => View("~/Views/HR/HRHome.cshtml");

        [Authorize(Policy = "EmployeeOnly")]
        public IActionResult EmployeeHome() => View("~/Views/Employee/EmployeeHome.cshtml");


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }

}
