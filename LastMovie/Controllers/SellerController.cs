using LastMovie.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class SellerController : Controller
    {
        AppDbContext db = new AppDbContext();

        public IActionResult Seller_Home()
        {
            var u = UserController.user.FirstOrDefault();
            var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
            var p = db.MovieProductTable.Where(x => x.RoleId == u.RoleId).ToList();
            double total = 0;
            double proco = 0;
            double quan = 0;
            double quantity = 0;
            foreach (var i in p)
            {
                total += i.BuyCount * i.MoviePrice;
                quantity += i.Quantity;
            }
            ViewBag.total = total.ToString("0.00");
            ViewBag.totalmiss = (total / 1000).ToString("0.00");
            ViewBag.pdCount = p.Count();
            proco = p.Count();
            ViewBag.pdCountmiss = (proco / 1000).ToString("0.00");
            ViewBag.quantity = quantity;
            ViewBag.quantitymiss = (quantity / 1000).ToString("0.00");
            List<Cart> c = new List<Cart>();
            foreach (var i in p)
            {
                var PM = db.CartTable.Where(x => x.MovieID == i.Movie_ID).ToList();
                c.AddRange(PM);

            }

            ViewBag.list = c.OrderByDescending(x => x.Quantity).Take(6).ToList();
            return View(newuser);
        }

        public IActionResult Seller_Product()
        {

            return View();
        }

        public IActionResult Seller_AddProduct()
        {
            var pro = db.MovieProductTable.OrderByDescending(x => x.Id).FirstOrDefault();
            if (pro == null)
            {
                ViewBag.pro_ID = "M-1";
            }
            else
            {
                int uid = pro.Id;

                var x = uid + 1;
                string xx = "M-" + x.ToString();
                ViewBag.pro_ID = xx;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Seller_AddProductAsync(List<IFormFile> file, MovieProduct info, string Video)
        {
            var user = UserController.user.FirstOrDefault();
            string imglist = "";
            for (int i = 0; i < file.Count; i++)
            {
                string filePath = $@"wwwroot/Image/MovieImg/{file[i].FileName}";
                imglist += file[i].FileName + "|";
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file[i].CopyToAsync(stream);
                };
            }

            imglist = imglist.Substring(0, imglist.LastIndexOf("|"));

            string newc = info.Category.Substring(0, info.Category.LastIndexOf(","));

            db.MovieProductTable.Add(new MovieProduct()
            {
                Movie_ID = info.Movie_ID,
                RoleId = user.RoleId,
                MovieImg = file[0].FileName,
                MovieName = info.MovieName,
                MovieInfo = info.MovieInfo,
                MoviePrice = info.MoviePrice,
                MovieDelPrice = info.MovieDelPrice,
                Quantity = info.Quantity,
                Category = newc,
                Like = 0,
                BuyCount = 0,
                IsInstallmentProduct = info.IsInstallmentProduct,
                ImgList = imglist
            });

            db.VideoTable.Add(new Video()
            {
                Movie_ID = info.Movie_ID,
                Youtube = Video,
                MovieImg = file[0].FileName,
                Like = 0,
                DisLike = 0,
                ImgList = imglist

            });

            db.SaveChanges();

            return RedirectToAction("Loading_Login_Seller_AddSuccess", "Loading");


        }


        public IActionResult Seller_Product_Edit(string ID)
        {
            var movie = db.MovieProductTable.Where(x => x.Movie_ID == ID).FirstOrDefault();
            var video = db.VideoTable.Where(x => x.Movie_ID == movie.Movie_ID).FirstOrDefault();
            ViewBag.Video = video.Youtube;
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit_Post(IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, MovieProduct info, string Video)
        {
            var movie = db.MovieProductTable.Where(x => x.Movie_ID == info.Movie_ID).FirstOrDefault();
            var dbVideo = db.VideoTable.Where(x => x.Movie_ID == info.Movie_ID).FirstOrDefault();
            var cart = db.CartTable.Where(x => x.MovieID == info.Movie_ID).ToList();
            string newc = info.Category.Substring(0, info.Category.LastIndexOf(","));
            List<string> imglist = new List<string>();
            List<IFormFile> list = new List<IFormFile>();
            string[] imglist1 = movie.ImgList.Split('|');
            for (int i = 0; i < imglist1.Count(); i++)
            {
                imglist.Add(imglist1[i]);
            }

            if (file1 != null)
            {
                movie.MovieImg = file1.FileName;
                dbVideo.MovieImg = file1.FileName;
                imglist[0] = file1.FileName;
                cart.ForEach(x => x.MovieImg = file1.FileName);
                list.Add(file1);
            }

            if (file2 != null)
            {
                if (imglist.Count() == 1)
                {
                    imglist.Add(file2.FileName);
                }
                else
                {
                    imglist[1] = file2.FileName;
                }
                list.Add(file2);
            }

            if (file3 != null)
            {
                if (imglist.Count() == 1)
                {
                    imglist.Add(file3.FileName);
                }
                else if (imglist.Count() == 2)
                {
                    imglist.Add(file3.FileName);
                }
                else
                {
                    imglist[2] = file3.FileName;
                }
                list.Add(file3);
            }

            if (file4 != null)
            {
                if (imglist.Count() == 1)
                {
                    imglist.Add(file4.FileName);
                }
                else if (imglist.Count() == 2)
                {
                    imglist.Add(file4.FileName);
                }
                else if (imglist.Count() == 3)
                {
                    imglist.Add(file4.FileName);
                }
                else
                {
                    imglist[3] = file4.FileName;
                }
                list.Add(file4);
            }

            for (int i = 0; i < list.Count(); i++)
            {
                string filePath = $@"wwwroot/Image/MovieImg/{list[i].FileName}";
                using (var stream = System.IO.File.Create(filePath))
                {
                    await list[i].CopyToAsync(stream);
                };
            }


            movie.MovieName = info.MovieName;
            movie.MovieInfo = info.MovieInfo;
            movie.MoviePrice = info.MoviePrice;
            movie.MovieDelPrice = info.MovieDelPrice;
            movie.Quantity = info.Quantity;
            movie.Category = newc;
            movie.IsInstallmentProduct = info.IsInstallmentProduct;
            dbVideo.Youtube = Video;


            string newimglist = "";
            for (int i = 0; i < imglist.Count(); i++)
            {
                newimglist += imglist[i] + "|";
            }
            newimglist = newimglist.Substring(0, newimglist.LastIndexOf("|"));

            movie.ImgList = newimglist;
            dbVideo.ImgList = newimglist;
            db.SaveChanges();

            return RedirectToAction("Loading_Login_Seller_Product", "Loading");
        }

        public IActionResult Seller_Edit_Page()
        {
            if (UserController.user.Count() == 0)
            {
                return RedirectToAction("HomePage", "MyHomeMovie");
            }
            var u = UserController.user.FirstOrDefault();
            var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
            return View(newuser);
        }

        [HttpPost]
        public async Task<IActionResult> Seller_Edit_Page(IFormFile file, User info)
        {
            var u = UserController.user.FirstOrDefault();

            if (file != null)
            {
                string filePath = $@"wwwroot/Image/UserHead/{u.Id + u.RoleId + file.FileName}";
                var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
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
                return RedirectToAction("Loading_Seller", "Loading");
            }
            else
            {
                var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
                newuser.Name = info.Name;
                newuser.Password = info.Password;
                newuser.Gmail = info.Gmail;
                newuser.Address = info.Address;
                newuser.AddressZip = info.AddressZip;
                newuser.Gender = info.Gender;
                newuser.City = info.City;
                db.SaveChanges();
                return RedirectToAction("Loading_Seller", "Loading");
            }
        }

        public IActionResult Seller_Message_Page()
        {
            return View();
        }
    }
}
