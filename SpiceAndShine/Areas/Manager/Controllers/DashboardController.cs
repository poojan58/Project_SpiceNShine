using Microsoft.AspNetCore.Mvc;
using SpiceAndShine.Areas.Manager.Models;

namespace SpiceAndShine.Areas.Manager.Controllers
{
    [Area("manager")]
    public class DashboardController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DashboardController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        DatabaseManager objDatabaseManager = new DatabaseManager();
        public IActionResult Index(bool IsRestricted = false)
        {
            if (IsRestricted)
            {
                ViewBag.Message = "You are now allowed to access it!";
                ViewBag.MessageType = "danger";
            }
          
            return View();
        }
    }
}
