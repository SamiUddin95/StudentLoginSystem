using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentLoginSystem.Models;
using StudentLoginSystem.Services;

namespace StudentLoginSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly GoogleDriveService _driveService;

        public DashboardController(GoogleDriveService driveService)
        {
            _driveService = driveService;
        }
        public IActionResult Index()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson)) return RedirectToAction("Login", "Account");

            var user = JsonConvert.DeserializeObject<UserModel>(userJson);

            // Set voucher link
            user.VoucherUrl = _driveService.GetVoucherUrlByRollNumber(user.RollNumber);

            return View(user);
        }
    }
}
