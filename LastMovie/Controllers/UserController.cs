using LastMovie.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class UserController : Controller
    {
        AppDbContext db = new AppDbContext();

        public static List<User> user = new List<User>();

        public IActionResult UserPage()
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }

            var u = user.FirstOrDefault();
            var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
            ViewBag.tr = db.TrTable.Where(x => x.RoleID == u.RoleId).Count();
            ViewBag.trs = db.TrTable.Where(x => x.RoleID == u.RoleId && x.TransactionStatus == "Paid").Count();
            ViewBag.trf = db.TrTable.Where(x => x.RoleID == u.RoleId && x.TransactionStatus == "Pending").Count();
            return View(newuser);
        }

        public IActionResult User_Tr_Page()
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            return View();
        }

        public IActionResult User_Cart_Page()
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            return View();
        }

        public IActionResult User_Message_Page()
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            return View();
        }

        public IActionResult User_Tr_Page_View(string ID)
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            var tr = db.TrTable.Where(x => x.TransactionId == ID).FirstOrDefault();
            return View(tr);
        }

        public IActionResult User_Cart_Buy(int Total)
        {

            ViewBag.Total = Total.ToString("0.00");
            return View();
        }

        public IActionResult User_Edit()
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            var u = user.FirstOrDefault();
            var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
            return View(newuser);
        }

        [HttpPost]
        public async Task<IActionResult> User_Edit(IFormFile file, User info)
        {
            var u = user.FirstOrDefault();

            if (file != null)
            {
                string filePath = $@"wwwroot/Image/UserHead/{u.Id + u.RoleId + file.FileName}";
                var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
                var cart = db.CartTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                cart.ForEach(x => x.UserName = info.Name);
                var tr = db.TrTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                tr.ForEach(x => x.Name = info.Name);
                newuser.Head = u.Id + u.RoleId + file.FileName;
                newuser.Name = info.Name;
                newuser.Password = info.Password;
                newuser.Gmail = info.Gmail;
                newuser.Address = info.Address;
                newuser.AddressZip = info.AddressZip;
                newuser.Gender = info.Gender;
                newuser.City = info.City;
                var comment = db.CommentTable.Where(x => x.User_Id == newuser.RoleId).ToList();
                comment.ForEach(x => x.Head = u.Id + u.RoleId + file.FileName);
                comment.ForEach(x => x.User_Name = info.Name);

                db.SaveChanges();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                };
                return RedirectToAction("Loading_Login_User_Home", "Loading");
            }
            else
            {
                var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
                var cart = db.CartTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                cart.ForEach(x => x.UserName = info.Name);
                var tr = db.TrTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                tr.ForEach(x => x.Name = info.Name);
                newuser.Name = info.Name;
                newuser.Password = info.Password;
                newuser.Gmail = info.Gmail;
                newuser.Address = info.Address;
                newuser.AddressZip = info.AddressZip;
                newuser.Gender = info.Gender;
                newuser.City = info.City;
                var comment = db.CommentTable.Where(x => x.User_Id == newuser.RoleId).ToList();
                comment.ForEach(x => x.User_Name = info.Name);
                db.SaveChanges();
                return RedirectToAction("Loading_Login_User_Home", "Loading");
            }
        }

        public IActionResult User_Installment()
        {

            return View();
        }

        public IActionResult User_ForgotPassword()
        {

            return View();
        }

        public IActionResult User_Installment_View(string TrID)
        {
            var tr = db.TrTable.Where(x => x.TransactionId == TrID).FirstOrDefault();
            var cart = db.CartTable.ToList();
            cart.ForEach(x => x.IsSelected = false);
            db.SaveChanges();
            return View(tr);
        }

        public IActionResult User_Installment_Pay(int InstallmentID)
        {
            ViewBag.InstallmentID = InstallmentID;
            var Installment = db.InstallmentRecordTable.Where(x => x.Id == InstallmentID).FirstOrDefault();
            ViewBag.Total = Installment.Amount.ToString("0.00");
            return View();
        }

        


    }
}
