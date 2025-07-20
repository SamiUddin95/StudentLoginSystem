using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentLoginSystem.Models;
using StudentLoginSystem.Services;
using System.Diagnostics.Eventing.Reader;

namespace StudentLoginSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly GoogleSheetService _sheetService;

        public AccountController(GoogleSheetService sheetService)
        {
            _sheetService = sheetService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string action)
        {
            if (action == "login")
            {
                var users = await _sheetService.GetUsersAsync();

                var user = users.FirstOrDefault(u =>
                    u.RollNumber == model.Username && u.Password == model.Password);

                if (user == null)
                {
                    ViewBag.Error = "Invalid credentials";
                    return View(model);
                }

                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                return RedirectToAction("Index", "Dashboard");
            }
            else if (action == "getPassword")
            {
                var users = _sheetService.GetUsersAsync().Result;
                var user = users.FirstOrDefault(u => u.RollNumber == model.Username);
                if (user != null)
                {
                    ViewBag.Password = user.Password;
                }
                else
                {
                    ViewBag.Error = "Roll number not found.";
                }

                return View(model);
            }
            return View(model);

        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetPassword(LoginViewModel model)
        {
            var users = _sheetService.GetUsersAsync().Result;

            var user = users.FirstOrDefault(u => u.RollNumber == model.Username);
            if (user != null)
            {
                ViewBag.Password = user.ProfileGrNo;
            }
            else
            {
                ViewBag.Error = "Roll number not found.";
            }

            return View("Login", model); // Re-render login view with result
        }

    }
}
