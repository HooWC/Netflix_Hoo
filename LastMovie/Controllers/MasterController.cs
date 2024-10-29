using LastMovie.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastMovie.Controllers
{
    public class MasterController : Controller
    {
        AppDbContext db = new AppDbContext();

        public IActionResult Master_Home()
        {
            ViewBag.meg = db.MessageTable.Count();
            ViewBag.com = db.CommentTable.Count();
            ViewBag.mypro = db.MovieProductTable.Where(x => x.RoleId == "Master").Count();
            ViewBag.sellerpro = db.MovieProductTable.Where(x => x.RoleId != "Master").Count();
            ViewBag.user = db.UserTable.Where(x => x.Role == "Customer").Count();
            ViewBag.seller = db.UserTable.Where(x => x.Role == "Seller").Count();
            var pd = db.MovieProductTable.Where(x => x.RoleId == "Master").ToList();
            long totalPro = 0;
            var dbpro = db.MovieProductTable.ToList();
            foreach (var i in dbpro)
            {
                totalPro += i.BuyCount;
            }
            double total = 0;
            double quantity = 0;
            foreach (var i in pd)
            {
                total += i.BuyCount * i.MoviePrice;
                quantity += i.Quantity;
            }
            ViewBag.total = total.ToString("0.00");
            ViewBag.quantity = quantity;
            ViewBag.totalProduct = db.MovieProductTable.Count();
            ViewBag.totalpro = totalPro;
            return View();
        }

        public IActionResult Master_Edit_Seller()
        {
            return View();
        }

        public IActionResult Master_Edit_User()
        {
            return View();
        }


        public IActionResult Master_Edit_Product()
        {
            return View();
        }


        public IActionResult Master_Message()
        {
            return View();
        }

        public IActionResult Master_Comment()
        {
            return View();
        }

        public IActionResult Master_MyProduct()
        {
            return View();
        }

        public IActionResult Master_MyProduct_Edit(string ID)
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

            if (movie.RoleId == "Master")
            {
                return RedirectToAction("Loading_Master_MyProduct", "Loading");
            }
            else
            {
                return RedirectToAction("Loading_Master_Seller_Product", "Loading");
            }
        }

        public IActionResult Master_seller_Product()
        {
            return View();
        }

        public IActionResult Master_Add_Movie()
        {
            var movie = db.MovieProductTable.OrderByDescending(x => x.Id).FirstOrDefault();
            if (movie == null)
            {
                ViewBag.ID = "M-1";
            }
            else
            {
                int uid = movie.Id;

                var x = uid + 1;
                string xx = "M-" + x.ToString();
                ViewBag.ID = xx;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Master_Add_Movie(List<IFormFile> file, MovieProduct info, string Video)
        {
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
                RoleId = "Master",
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

            return RedirectToAction("Loading_Master_Home", "Loading");

        }

        public IActionResult Master_User()
        {
            return View();
        }

        public IActionResult Edit_User(string Uid)
        {
            var user = db.UserTable.Where(x => x.RoleId == Uid).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit_User(IFormFile file, User info)
        {
            var u = db.UserTable.Where(x => x.RoleId == info.RoleId).FirstOrDefault();

            if (file != null)
            {
                string filePath = $@"wwwroot/Image/UserHead/{u.Id + u.RoleId + file.FileName}";
                var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
                var tr = db.TrTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                tr.ForEach(x => x.Name = info.Name);
                var cart = db.CartTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                cart.ForEach(x => x.UserName = info.Name);
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

                if (newuser.Role == "Seller")
                {
                    return RedirectToAction("Loading_Master_Seller", "Loading");
                }
                else
                {
                    return RedirectToAction("Loading_Master_User", "Loading");
                }


            }
            else
            {
                var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
                var tr = db.TrTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                tr.ForEach(x => x.Name = info.Name);
                var cart = db.CartTable.Where(x => x.RoleID == newuser.RoleId).ToList();
                cart.ForEach(x => x.UserName = info.Name);
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

                if (newuser.Role == "Seller")
                {
                    return RedirectToAction("Loading_Master_Seller", "Loading");
                }
                else
                {
                    return RedirectToAction("Loading_Master_User", "Loading");
                }
            }
        }

        public IActionResult Master_Seller()
        {
            return View();
        }

        public IActionResult Delete_User(string Uid)
        {
            var user = db.UserTable.Where(x => x.RoleId == Uid).FirstOrDefault();
            db.UserTable.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Loading_Master_Del_User", "Loading");
        }

        [HttpPost]
        public IActionResult Delete_Conn(string ID)
        {
            var conn = db.CommentTable.Where(x => x.ConnID == ID).FirstOrDefault();
 
            try
            {
                if (conn != null)
                {
                    db.CommentTable.Remove(conn);
                }
                db.SaveChanges();
            }
            catch
            {
                var conn1 = db.CommentTable.OrderByDescending(x => x.Id).ToList();
                return Json(conn1);
            }

            var conn2 = db.CommentTable.OrderByDescending(x => x.Id).ToList();
            return Json(conn2);
        }

        public IActionResult Delete_Seller(string Uid)
        {
            var user = db.UserTable.Where(x => x.RoleId == Uid).FirstOrDefault();
            db.UserTable.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Loading_Master_Del_Seller", "Loading");
        }

        public IActionResult Master_Analyze()
        {
            return View();
        }

        public IActionResult Master_Analyze_Product()
        {
            return View();
        }

        public IActionResult Master_Analyze_Pro_Product_Price()
        {
            return View();
        }

        public IActionResult Master_Analyze_User()
        {
            return View();
        }

        public IActionResult Master_Analyze_Seller()
        {
            return View();
        }

        public IActionResult Master_All_Message()
        {
            return View();
        }


    }
}
