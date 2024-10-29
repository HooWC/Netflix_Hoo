using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult ContactUsPage()
        {
            return View();
        }
    }
}
