using LastMovie.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class LoginController : Controller
    {
        AppDbContext db = new AppDbContext();

        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult SignUpPage()
        {
            var user = db.UserTable.OrderByDescending(x => x.Id).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ID = "U-1";
            }
            else
            {
                int uid = user.Id;

                var x = uid + 1;
                string xx = "U-" + x.ToString();
                ViewBag.ID = xx;
            }
            return View();
        }

        public IActionResult SignOut()
        {

            UserController.user.Clear();

            return RedirectToAction("Home_Home", "MyHomeMovie");
        }


    }
}
