using LastMovie.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class MyMovieProductController : Controller
    {
        AppDbContext db = new AppDbContext();

        public IActionResult Product_Home()
        {
            if (UserController.user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            return View();
        }

        public IActionResult Product_List()
        {
            if (UserController.user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            return View();
        }

        public IActionResult Product_Page(string ID)
        {
            if (UserController.user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            var p = db.MovieProductTable.Where(x => x.Movie_ID == ID).FirstOrDefault();
            var myvideo = db.VideoTable.Where(x => x.Movie_ID == ID).FirstOrDefault();
            ViewBag.Video = myvideo.Youtube;
            var user = UserController.user.FirstOrDefault();
            var newuser = db.UserTable.Where(x => x.RoleId == user.RoleId).FirstOrDefault();
            ViewBag.Head = newuser.Head;
            ViewBag.UserName = newuser.Name;
            return View(p);
        }
    }
}
