using LastMovie.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class MyHomeMovieController : Controller
    {


        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Home_Home()
        {
            return View();
        }
    }
}
