using Microsoft.AspNetCore.Mvc;
using SpiceAndShine.Areas.Manager.Models;
using SpiceAndShine.Models;

namespace SpiceAndShine.Areas.Manager.Controllers
{
    [Area("manager")]
    public class LoginController : Controller
    {
        DatabaseManager objDatabaseManager = new DatabaseManager();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("restaurant")]
        public IActionResult Login(ManagerLoginModel managerLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = "";
                    bool LoginComplete = false;
                    ManagerLoginResult managerLoginResult = new ManagerLoginResult();

                    managerLoginResult = objDatabaseManager.ManagerLogin(managerLoginModel);

                    if (managerLoginResult.Flag == 1)
                    {
                        result = Common.Messages.UserNotAvailable;
                    }
                    else if (managerLoginResult.Flag == 2)
                    {
                        result = Common.Messages.RestaurantIsNotActive;
                    }
                    else if (managerLoginResult.Flag == 3)
                    {
                        result = Common.Messages.IncorrectPassword;
                    }
                    else if (managerLoginResult.Flag == 4)//successfully login
                    {
                        HttpContext.Session.SetComplexData(Common.SessionKeys.ManagerSession, managerLoginResult.ManagerData);
                        var abc = HttpContext.Session.GetComplexData<ManagerSession>(Common.SessionKeys.ManagerSession);
                    }
                    else
                    {
                        result = Common.Messages.LoginFailed;
                    }
                    return Json(new
                    {
                        status = Convert.ToInt32(managerLoginResult.Flag),
                        message = result
                    });
                }
                else
                {
                    return PartialView(managerLoginModel);
                }
            }
            catch (Exception ex) { throw; }
        }

    }
}
