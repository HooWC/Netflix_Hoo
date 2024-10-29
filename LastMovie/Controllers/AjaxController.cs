using LastMovie.CreditCardFunction;
using LastMovie.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Stripe;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace LastMovie.Controllers
{
	public class AjaxController : Controller
	{
		AppDbContext db = new AppDbContext();



		private readonly DtoStripeSecrets _stripeSecrets;

		static DtoStripeSecrets stripeSecrets = new DtoStripeSecrets()
		{
			SecretKey = "sk_test_51LFrBPCDMjmOjphDkCD5knDL4oLcb1bb6XMIOHIZ3AizXlVnzdS6xcgVYbkxPTdg35VrT5Rd5xME04Cq5PoOt7D100pS55JR49",
			PublishableKey = "pk_test_51LFrBPCDMjmOjphDmhCWgsFP5QXCXxv0p1oFE0ySjBUHQ0CwKAeuxeCNEYl8gtkXo4NZsYTDiD8Hf0KPQKVUA2mJ00FWY79rOk"
		};



		public static List<Cart> cart = new List<Cart>();

		public static string Tr_ID = "";

		public List<MovieProduct> pd = new List<MovieProduct>();

		public IActionResult GetProduct_Home_Anime()
		{
			var data = db.MovieProductTable.Where(x => x.Category.Contains("Anime") && x.Quantity > 0).Take(8);
			return Json(data);
		}

		public IActionResult GetProduct_Home_Action()
		{
			var data = db.MovieProductTable.Where(x => x.Category.Contains("Action") && x.Quantity > 0).Take(8);
			return Json(data);
		}

		public IActionResult GetProduct_Home_Romance()
		{
			var data = db.MovieProductTable.Where(x => x.Category.Contains("Romance") && x.Quantity > 0).Take(8);
			return Json(data);
		}

		public IActionResult GetProduct_Home_Fantasy()
		{
			var data = db.MovieProductTable.Where(x => x.Category.Contains("Fantasy") && x.Quantity > 0).OrderByDescending(x => x.Id).Take(8);
			return Json(data);
		}

		public IActionResult GetProduct_List()
		{
			pd = db.MovieProductTable.Where(x => x.Quantity > 0).ToList();
			return Json(pd);
		}

		public IActionResult GetProduct_List_Filter(string myselect)
		{
			if (myselect == "All Categories")
			{
				pd = db.MovieProductTable.Where(x => x.Quantity > 0).ToList();
				return Json(pd);
			}
			else
			{
				pd = db.MovieProductTable.Where(x => x.Category.Contains(myselect) && x.Quantity > 0).ToList();
				return Json(pd);
			}


		}

		[HttpPost]
		public IActionResult GetProduct_List_Search(string Search)
		{
			if (Search == null)
			{
				pd = db.MovieProductTable.Where(x => x.Quantity > 0).ToList();
				return Json(pd);
			}
			else
			{
				var productName = db.MovieProductTable.Where(x => x.MovieName.ToLower().Contains(Search.ToLower()) && x.Quantity > 0).ToList();
				if (productName.Count() == 0)
				{
					return Json(db.MovieProductTable);
				}
				return Json(productName);
			}
		}

		[HttpPost]
		public IActionResult GetProduct_List_Search_Master(string Search)
		{
			if (Search == null)
			{
				pd = db.MovieProductTable.Where(x => x.Quantity > 0 && x.RoleId == "Master").ToList();
				return Json(pd);
			}
			else
			{
				var productName = db.MovieProductTable.Where(x => x.MovieName.ToLower().Contains(Search.ToLower()) && x.Quantity > 0 && x.RoleId == "Master").ToList();
				if (productName.Count() == 0)
				{
					pd = db.MovieProductTable.Where(x => x.Quantity > 0 && x.RoleId == "Master").ToList();
					return Json(pd);
				}
				return Json(productName);
			}
		}

		[HttpPost]
		public IActionResult GetProduct_List_Search_SellerPage_Product(string Search)
		{
			var user = UserController.user.FirstOrDefault();
			if (Search == null)
			{
				pd = db.MovieProductTable.Where(x => x.Quantity > 0 && x.RoleId == user.RoleId).ToList();
				return Json(pd);
			}
			else
			{
				var productName = db.MovieProductTable.Where(x => x.MovieName.ToLower().Contains(Search.ToLower()) && x.Quantity > 0 && x.RoleId == user.RoleId).ToList();
				if (productName.Count() == 0)
				{
					pd = db.MovieProductTable.Where(x => x.Quantity > 0 && x.RoleId == user.RoleId).ToList();
					return Json(pd);
				}
				return Json(productName);
			}
		}

		[HttpPost]
		public IActionResult GetProduct_List_Search_User(string Search)
		{
			if (Search == null)
			{
				var us = db.UserTable.Where(x => x.Role == "Customer").ToList();
				return Json(us);
			}
			else
			{
				var productName = db.UserTable.Where(x => x.Name.ToLower().Contains(Search.ToLower()) && x.Role == "Customer").ToList();
				if (productName.Count() == 0)
				{
					var us = db.UserTable.Where(x => x.Role == "Customer").ToList();
					return Json(us);
				}
				return Json(productName);
			}
		}

		[HttpPost]
		public IActionResult GetProduct_List_Search_Seller1(string Search)
		{
			if (Search == null)
			{
				var us = db.UserTable.Where(x => x.Role == "Seller").ToList();
				return Json(us);
			}
			else
			{
				var productName = db.UserTable.Where(x => x.Name.ToLower().Contains(Search.ToLower()) && x.Role == "Seller").ToList();
				if (productName.Count() == 0)
				{
					var us = db.UserTable.Where(x => x.Role == "Seller").ToList();
					return Json(us);
				}
				return Json(productName);
			}
		}

		[HttpPost]
		public IActionResult GetProduct_List_Search_Seller(string Search)
		{
			if (Search == null)
			{
				pd = db.MovieProductTable.Where(x => x.Quantity > 0 && x.RoleId != "Master").ToList();
				return Json(pd);
			}
			else
			{
				var productName = db.MovieProductTable.Where(x => x.MovieName.ToLower().Contains(Search.ToLower()) && x.Quantity > 0 && x.RoleId != "Master").ToList();
				if (productName.Count() == 0)
				{
					pd = db.MovieProductTable.Where(x => x.Quantity > 0 && x.RoleId != "Master").ToList();
					return Json(pd);
				}
				return Json(productName);
			}
		}

		[HttpPost]
		public IActionResult SignUp_Post(string Sellgm)
		{

			if (Sellgm == null)
			{
				return Json(false);
			}
			else
			{
				if (Sellgm.Length < 6)
				{
					return Json(false);
				}
				else
				{
					var xm = db.UserTable.Where(x => x.Password == Sellgm).FirstOrDefault();
					if (xm != null)
					{
						return Json(false);
					}
					else
					{
						return Json(true);
					}
				}
			}

		}

		[HttpPost]
		public IActionResult SignUp_Post_Gmail(string Sellgm)
		{

			if (Sellgm == null)
			{
				return Json(false);
			}
			else
			{
				if (!Sellgm.Contains("@gmail.com"))
				{
					return Json(false);
				}
				else
				{
					var xm = db.UserTable.Where(x => x.Gmail == Sellgm).FirstOrDefault();
					if (xm != null)
					{
						return Json(false);
					}
					else
					{
						return Json(true);
					}
				}
			}

		}

		[HttpPost]
		public IActionResult SignUp(User user)
		{
			db.UserTable.Add(new User()
			{
				RoleId = user.RoleId,
				Role = user.Role,
				Name = user.Name,
				Gender = user.Gender,
				Gmail = user.Gmail,
				Password = user.Password,
				Address = user.Address,
				AddressZip = user.AddressZip,
				City = user.City,
				CreateDate = DateTime.Now,
				Head = "User2.png",
				Money = 0
			});

			db.SaveChanges();

			return Json(true);

		}

		[HttpPost]
		public IActionResult LoginPost(string myGmail, string myPassword)
		{
			if (myGmail == "MasterID" && myPassword == "084487")
			{
				return Json("Master");
			}

			var checkUser = db.UserTable.Where(x => x.Gmail == myGmail && x.Password == myPassword).FirstOrDefault();
			if (checkUser == null)
			{
				return Json(false);
			}
			else
			{
				UserController.user = db.UserTable.Where(x => x.Gmail == myGmail && x.Password == myPassword).ToList();
				var user = UserController.user.FirstOrDefault();
				if (user.Role == "Customer")
				{
					return Json(true);
				}
				else if (user.Role == "Seller")
				{
					return Json("seller");
				}
				else
				{
					return Json(false);
				}

			}
		}

		[HttpPost]
		public IActionResult AddMovieToCart(string ProductID)
		{
			var movie = db.MovieProductTable.Where(x => x.Movie_ID == ProductID).FirstOrDefault();
			var u = UserController.user.FirstOrDefault();
			var Addcart = db.CartTable.Where(x => x.RoleID == u.RoleId && x.MovieID == movie.Movie_ID).FirstOrDefault();
			var Addcart2 = db.CartTable.Where(x => x.RoleID == u.RoleId && x.InstallmentLastBuy == true).FirstOrDefault();
			var Addcart3 = db.CartTable.Where(x => x.RoleID == u.RoleId && x.MovieID == ProductID && x.InstallmentLastBuy == true).FirstOrDefault();
			//var tr = db.TrTable.Where(x => x.RoleID == u.RoleId && x.InstallmentStatus == false && x.TransactionStatus == "Paid").FirstOrDefault();

			if (Addcart3 != null)
			{
				return Json("Installment");
			}
			else
			{
				if (Addcart == null)
				{
					db.CartTable.Add(new Cart()
					{
						RoleID = u.RoleId,
						UserName = u.Name,
						MovieImg = movie.MovieImg,
						MovieName = movie.MovieName,
						MovieID = movie.Movie_ID,
						Quantity = 1,
						LastQuantity = 1,
						MoviePrice = movie.MoviePrice,
						Buy = false,
						IsSelected = false,
						InstallmentTime = 0,
						IsInstallmentProduct = movie.IsInstallmentProduct,
						InstallmentLastTime = 0,
						InstallmentLastBuy = false
					});


					movie.Quantity -= 1;
					//movie.BuyCount += 1;
				}
				else
				{
					if (Addcart.Buy == true)
					{
						Addcart.Quantity += 1;
						Addcart.LastQuantity = 1;
						Addcart.Buy = false;
						Addcart.IsSelected = false;
					}
					else if (Addcart.Buy == false)
					{
						Addcart.Quantity += 1;
						Addcart.LastQuantity += 1;
						Addcart.Buy = false;
						Addcart.IsSelected = false;

					}

					movie.Quantity -= 1;
					//movie.BuyCount += 1;

				}
			}



			db.SaveChanges();

			return Json(true);
		}

		public IActionResult GetCartInfo()
		{
			var u = UserController.user.FirstOrDefault();
			var cartMovie = db.CartTable.Where(x => x.RoleID == u.RoleId && x.Buy == false && x.InstallmentLastBuy == false).ToList();

			if (cartMovie.Count() == 0)
			{
				return Json(false);
			}
			else
			{
				return Json(cartMovie);
			}
		}

		[HttpPost]
		public IActionResult MinusPost(int ID)
		{
			var u = UserController.user.FirstOrDefault();
			var cart = db.CartTable.Where(x => x.RoleID == u.RoleId && x.Id == ID).FirstOrDefault();
			var movie = db.MovieProductTable.Where(x => x.Movie_ID == cart.MovieID).FirstOrDefault();
			movie.Quantity += 1;
			//movie.BuyCount -= 1;
			cart.LastQuantity -= 1;
			cart.Quantity -= 1;

			if (cart.Quantity == 0 && cart.LastQuantity == 0)
			{
				if (cart.IsInstallmentProduct == true)
				{
					cart.InstallmentTime = 0;
					cart.InstallmentLastTime = 0;
				}
				db.Remove(cart);
			}
			if (cart.Quantity != 0 && cart.LastQuantity == 0)
			{
				if (cart.IsInstallmentProduct == true)
				{
					cart.InstallmentTime = 0;
					cart.InstallmentLastTime = 0;
				}
				cart.Buy = true;
			}

			db.SaveChanges();


			var u2 = UserController.user.FirstOrDefault();
			var cartMovie = db.CartTable.Where(x => x.RoleID == u2.RoleId && x.Buy == false && x.InstallmentLastBuy == false).ToList();

			if (cartMovie.Count() == 0)
			{
				return Json(false);
			}
			else
			{
				return Json(cartMovie);
			}
		}

		[HttpPost]
		public IActionResult AddPost(int ID)
		{

			var u = UserController.user.FirstOrDefault();
			var cart = db.CartTable.Where(x => x.RoleID == u.RoleId && x.Id == ID).FirstOrDefault();
			var movie = db.MovieProductTable.Where(x => x.Movie_ID == cart.MovieID).FirstOrDefault();
			if (movie.Quantity == 0)
			{

			}
			else
			{
				movie.Quantity -= 1;
				//movie.BuyCount += 1;
				cart.LastQuantity += 1;
				cart.Quantity += 1;
			}

			db.SaveChanges();

			var u2 = UserController.user.FirstOrDefault();
			var cartMovie = db.CartTable.Where(x => x.RoleID == u2.RoleId && x.Buy == false && x.InstallmentLastBuy == false).ToList();


			return Json(cartMovie);
		}

		[HttpPost]
		public IActionResult MyDelete(int ID)
		{

			var u = UserController.user.FirstOrDefault();
			var cart = db.CartTable.Where(x => x.RoleID == u.RoleId && x.Id == ID).FirstOrDefault();
			var movie = db.MovieProductTable.Where(x => x.Movie_ID == cart.MovieID).FirstOrDefault();
			movie.Quantity += cart.LastQuantity;
			//movie.BuyCount -= cart.LastQuantity;

			if (cart.Quantity - cart.LastQuantity <= 0)
			{
				if (cart.IsInstallmentProduct == true)
				{
					cart.InstallmentTime = 0;
					cart.InstallmentLastTime = 0;
				}
				db.CartTable.Remove(cart);
			}
			else if (cart.Quantity - cart.LastQuantity != 0)
			{
				if (cart.IsInstallmentProduct == true)
				{
					cart.InstallmentTime = 0;
					cart.InstallmentLastTime = 0;
				}
				cart.Buy = true;
				cart.LastQuantity = 0;
				cart.Quantity -= cart.LastQuantity;
			}

			db.SaveChanges();

			var u2 = UserController.user.FirstOrDefault();
			var cartMovie = db.CartTable.Where(x => x.RoleID == u2.RoleId && x.Buy == false && x.InstallmentLastBuy == false).ToList();

			if (cartMovie.Count() == 0)
			{
				return Json(false);
			}
			else
			{
				return Json(cartMovie);
			}


		}

		[HttpPost]
		public IActionResult MakeNewPY(double TotalPricePost, string CartListPost)
		{
			if (CartListPost == null || TotalPricePost == 0)
			{
				return Json(false);
			}
			var u = UserController.user.FirstOrDefault();
			var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();

			var tr = db.TrTable.OrderByDescending(x => x.Id).FirstOrDefault();
			if (tr == null)
			{
				ViewBag.tr_ID = "T-1";
				Tr_ID = "T-1";
			}
			else
			{
				int uid = tr.Id;

				var x = uid + 1;
				string xx = "T-" + x.ToString();
				ViewBag.tr_ID = xx;
				Tr_ID = xx;
			}

			cart.Clear();
			string[] str = CartListPost.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var i in str)
			{
				var c = db.CartTable.Where(x => x.Id == Convert.ToInt32(i)).FirstOrDefault();
				cart.Add(c);
			}

			var newCartList = "";
			double total = 0;
			foreach (var i in cart)
			{
				if (i.InstallmentLastTime == 0 || i.InstallmentLastBuy == false)
				{
					newCartList += i.Id + "|";
					total += i.MoviePrice;
				}
			}

			if (newCartList != "")
			{
				db.TrTable.Add(new Tr()
				{
					TransactionId = ViewBag.tr_ID,
					RoleID = newuser.RoleId,
					Name = newuser.Name,
					BeforeTotal = 0,
					Total = total,
					Date = DateTime.Now,
					PaidTime = DateTime.Now.ToString(),
					TransactionStatus = "Pending",
					CartList = newCartList,
					BillingAddress = newuser.Address,
					Tax = 0,
					InstallmentStatus = true
				});


			}


			db.SaveChanges();
			return Json(true);
		}

		[HttpPost]
		public IActionResult PY(double Total, CreditCard data)
		{
			if (data.number != "4242424242424242" || data.name == null || data.Cvc == null || data.ExpMonth == 0 || data.expYear == 0)
			{
				return Json(false);
			}
			var u = UserController.user.FirstOrDefault();
			var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();

			try
			{
				long amount = Convert.ToInt64(Total * 100);

				StripePayment stripePayment = new StripePayment(new CreditCard
				{
					Name = data.Name,
					Email = newuser.Gmail,
					AddressLine1 = newuser.Address,
					AddressLine2 = newuser.Address,
					AddressCity = newuser.City,
					AddressState = "True",
					AddressZip = newuser.AddressZip,//地址邮编
					Descripcion = $"Purchase on {DateTime.Now}",
					DetailsDescripcion = $"test {DateTime.Now:d}",
					Amount = amount, //2000 = 20.00   10000 == ??
					Currency = "MYR",
					Number = data.number,//4242 4242 4242 4242
					ExpMonth = data.ExpMonth,
					ExpYear = data.ExpYear,
					Cvc = data.Cvc //123
				}, stripeSecrets);
				Charge charge = stripePayment.ProccessPayment();

				var selectCard = db.CartTable.Where(x => x.RoleID == u.RoleId && x.IsSelected == true && x.Buy == false).ToList();
				bool check = true;
				foreach (var i in selectCard)
				{
					// Not Installment
					if (i.InstallmentTime == 0 && i.IsInstallmentProduct == false && i.InstallmentLastTime == 0)
					{
						i.Buy = true;
						var p = db.MovieProductTable.Where(x => x.Movie_ID == i.MovieID).FirstOrDefault();
						p.BuyCount += i.LastQuantity;
						i.LastQuantity = 0;
						var tr_check = db.TrTable.OrderByDescending(x => x.TransactionId == Tr_ID).FirstOrDefault();
						tr_check.TransactionStatus = "Paid";
						tr_check.InstallmentStatus = true;
						tr_check.PaidTime = DateTime.Now.ToString();
					}
					else if (i.InstallmentTime == 0 && i.IsInstallmentProduct == true && i.InstallmentLastTime == 0)
					{
						i.Buy = true;
						var p = db.MovieProductTable.Where(x => x.Movie_ID == i.MovieID).FirstOrDefault();
						p.BuyCount += i.LastQuantity;
						i.LastQuantity = 0;
						var tr_check = db.TrTable.OrderByDescending(x => x.TransactionId == Tr_ID).FirstOrDefault();
						tr_check.TransactionStatus = "Paid";
						tr_check.InstallmentStatus = true;
						tr_check.PaidTime = DateTime.Now.ToString();
					}
					else if (i.InstallmentTime != 0 && i.IsInstallmentProduct == true && i.InstallmentLastTime - 1 != 0)
					{
						string GiveTr = Tr_ID;
						string search = i.Id.ToString();

						if (i.InstallmentLastBuy == false)
						{
							for (int x = 0; x < i.InstallmentTime; x++)
							{
								db.InstallmentRecordTable.Add(new InstallmentRecord()
								{
									User_ID = u.RoleId,
									Tr_ID = GiveTr,
									PaymentStartDate = DateTime.Now.AddMonths(x).ToString(),
									PaymentDueDate = DateTime.Now.AddMonths(x + 1).ToString(),
									Status = (x == 0) ? "Installment Paid" : "Pending",
									Amount = i.MoviePrice / i.InstallmentTime,
									PaidTime = (x == 0) ? DateTime.Now.ToString() : "Not Yet Pay",
									Movie_ID = i.MovieID
								});
							}

							var newtr_first = db.TrTable.Where(x => x.TransactionId == GiveTr).FirstOrDefault();
							newtr_first.TransactionStatus = "Installments";
							newtr_first.InstallmentStatus = false;
						}
						else if (i.InstallmentLastBuy == true)
						{
							var newtr = db.TrTable.Where(x => x.RoleID == u.RoleId && x.TransactionStatus == "Installments" && x.CartList.Contains(search)).FirstOrDefault();
							var findInstallmentRecord = db.InstallmentRecordTable.Where(x => x.Tr_ID == newtr.TransactionId && x.Status == "Pending").FirstOrDefault();
							if (findInstallmentRecord != null)
							{
								findInstallmentRecord.Status = "Installment Paid";
								findInstallmentRecord.PaidTime = DateTime.Now.ToString();
							}
						}

						i.InstallmentLastTime -= 1;
						i.InstallmentLastBuy = true;
						i.IsSelected = false;
						i.Buy = false;

					}
					else if (i.InstallmentTime != 0 && i.IsInstallmentProduct == true && i.InstallmentLastTime - 1 == 0)
					{
						//最后一次付款
						string search = i.Id.ToString();
						var newtr = db.TrTable.Where(x => x.RoleID == u.RoleId && x.TransactionStatus == "Installments" && x.CartList.Contains(search)).FirstOrDefault();
						var findInstallmentRecord = db.InstallmentRecordTable.Where(x => x.Tr_ID == newtr.TransactionId && x.Status == "Pending").FirstOrDefault();
						if (findInstallmentRecord != null)
						{
							findInstallmentRecord.Status = "Installment Paid";
							findInstallmentRecord.PaidTime = DateTime.Now.ToString();
							db.SaveChanges();
						}

						var LastfindInstallmentRecord = db.InstallmentRecordTable.Where(x => x.Status == "Pending" && x.Tr_ID == newtr.TransactionId).FirstOrDefault();
						if (LastfindInstallmentRecord == null)
						{
							var changeTR = db.TrTable.Where(x => x.TransactionId == newtr.TransactionId).FirstOrDefault();
							changeTR.TransactionStatus = "Paid";
							changeTR.InstallmentStatus = true;
							changeTR.PaidTime = DateTime.Now.ToString();
						}

						i.InstallmentTime = 0;

						i.InstallmentLastTime = 0;
						i.InstallmentLastBuy = false;
						i.Buy = true;
						var p = db.MovieProductTable.Where(x => x.Movie_ID == i.MovieID).FirstOrDefault();
						p.BuyCount += i.LastQuantity;
						i.LastQuantity = 0;

					}

				}
				db.SaveChanges();

				try
				{
					string form = "wengchin1234567@gamil.com";
					MailMessage m = new MailMessage(form, newuser.Gmail);
					string date = DateTime.Now.ToString();

					LinkedResource LinkedImage = new LinkedResource(@".\007.png");
					LinkedImage.ContentId = "MyPic1";


					LinkedImage.ContentType = new System.Net.Mime.ContentType(MediaTypeNames.Image.Jpeg);


					AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
					   "<h1>Thank you for your purchase and support</h1>", null, "text/html"
					   );

					htmlView.LinkedResources.Add(LinkedImage);
					m.AlternateViews.Add(htmlView);

					m.Subject = "Thank You";
					m.BodyEncoding = Encoding.UTF8;
					m.IsBodyHtml = true;

					System.Net.Mail.SmtpClient c = new SmtpClient("smtp.gmail.com", 587);
					NetworkCredential n = new NetworkCredential("wengchin1234567@gmail.com", "ufznhwwqjoziiztw");

					c.EnableSsl = true;
					c.UseDefaultCredentials = false;
					c.Credentials = n;

					c.Send(m);

				}
				catch (Exception ex)
				{

				}

			}
			catch (Exception ex)
			{
				return Json(ex.Message);
			}


			return Json(true);
		}

		public IActionResult GetTr()
		{
			var u = UserController.user.FirstOrDefault();
			var tr = db.TrTable.Where(x => x.RoleID == u.RoleId).OrderByDescending(x => x.Id).ToList();

			return Json(tr);
		}

		public IActionResult GetTr_View(string RoleID)
		{
			var tr = db.TrTable.Where(x => x.TransactionId == RoleID).FirstOrDefault();
			cart.Clear();
			string[] str = tr.CartList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var i in str)
			{
				var c = db.CartTable.Where(x => x.Id == Convert.ToInt32(i)).FirstOrDefault();
				cart.Add(c);
			}
			return Json(cart);
		}

		public IActionResult GetComment(string MovieID)
		{
			var product = db.MovieProductTable.Where(x => x.Movie_ID == MovieID).FirstOrDefault();
			var com = db.CommentTable.Where(x => x.Movie_Id == product.Movie_ID).OrderByDescending(x => x.Id).ToList();
			return Json(com);
		}

		[HttpPost]
		public IActionResult CommentPost(string MovieID, string Comment)
		{
			var u = UserController.user.FirstOrDefault();
			var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
			var product = db.MovieProductTable.Where(x => x.Movie_ID == MovieID).FirstOrDefault();

			var c = db.CommentTable.OrderByDescending(x => x.Id).FirstOrDefault();

			if (c == null)
			{
				ViewBag.ID = "C-1";
			}
			else
			{
				int uid = c.Id;

				var x = uid + 1;
				string xx = "C-" + x.ToString();
				ViewBag.ID = xx;
			}


			db.CommentTable.Add(new Comment()
			{
				Movie_Id = MovieID,
				User_Id = newuser.RoleId,
				Like = 0,
				DisLike = 0,
				CreatedDate = DateTime.Now.ToString(),
				CommentWord = Comment,
				Head = newuser.Head,
				User_Name = newuser.Name,
				ConnID = ViewBag.ID
			});

			db.SaveChanges();

			var com = db.CommentTable.Where(x => x.Movie_Id == product.Movie_ID).OrderByDescending(x => x.Id).ToList();

			return Json(com);


		}

		public IActionResult GetSellerProduct()
		{
			var user = UserController.user.FirstOrDefault();
			var newuser = db.UserTable.Where(x => x.RoleId == user.RoleId).FirstOrDefault();
			var pro = db.MovieProductTable.Where(x => x.RoleId == newuser.RoleId).ToList();
			return Json(pro);
		}

		[HttpPost]
		public IActionResult Seller_Create_Product(MovieProduct movie, string Video)
		{
			var user = UserController.user.FirstOrDefault();

			string newc = movie.Category.Substring(0, movie.Category.LastIndexOf(","));

			db.MovieProductTable.Add(new MovieProduct()
			{
				Movie_ID = movie.Movie_ID,
				RoleId = user.RoleId,
				MovieImg = movie.MovieImg,
				MovieName = movie.MovieName,
				MovieInfo = movie.MovieInfo,
				MoviePrice = movie.MoviePrice,
				MovieDelPrice = movie.MovieDelPrice,
				Quantity = movie.Quantity,
				Category = newc,
				Like = 0,
				BuyCount = 0,
				IsInstallmentProduct = movie.IsInstallmentProduct
			});

			db.VideoTable.Add(new Video()
			{
				Movie_ID = movie.Movie_ID,
				Youtube = Video,
				MovieImg = movie.MovieImg,
				Like = 0,
				DisLike = 0
			});


			db.SaveChanges();

			return Json(true);


		}

		[HttpPost]
		public IActionResult CheckSelectCart(bool Check, int ID)
		{
			var user = UserController.user.FirstOrDefault();
			var cart = db.CartTable.Where(x => x.Id == ID).FirstOrDefault();
			if (cart.InstallmentTime != 0)
			{
				var cart2 = db.CartTable.Where(x => x.RoleID == user.RoleId && x.Buy == false && x.InstallmentTime == 0).ToList();
				cart2.ForEach(x => x.IsSelected = false);
			}
			cart.IsSelected = Check;
			db.SaveChanges();

			var u2 = UserController.user.FirstOrDefault();
			var cartMovie = db.CartTable.Where(x => x.RoleID == u2.RoleId && x.Buy == false && x.InstallmentLastBuy == false).ToList();

			if (cartMovie.Count() == 0)
			{
				return Json(false);
			}
			else
			{
				return Json(cartMovie);
			}
		}

		[HttpPost]
		public IActionResult InstallmentSelect(int SelectValue, int ID)
		{

			var cart = db.CartTable.Where(x => x.Id == ID).FirstOrDefault();
			if (cart.InstallmentTime != 0 && cart.IsSelected == true)
			{
				if (SelectValue == 0)
				{
					cart.IsSelected = false;
				}
			}
			cart.InstallmentTime = SelectValue;
			cart.InstallmentLastTime = SelectValue;

			var user = UserController.user.FirstOrDefault();
			if (cart.InstallmentTime != 0 && cart.IsSelected == true)
			{
				var cart2 = db.CartTable.Where(x => x.Id != ID && x.RoleID == user.RoleId && x.Buy == false && x.InstallmentTime == 0).ToList();
				cart2.ForEach(x => x.IsSelected = false);
			}

			db.SaveChanges();

			var u = UserController.user.FirstOrDefault();
			var cartMovie = db.CartTable.Where(x => x.RoleID == u.RoleId && x.Buy == false && x.InstallmentLastBuy == false).ToList();

			return Json(cartMovie);


		}

		[HttpPost]
		public IActionResult Edit_Post_Password(string Sellgm)
		{
			var u = UserController.user.FirstOrDefault();
			if (Sellgm == null)
			{
				return Json(false);
			}
			else
			{
				if (Sellgm.Length < 6)
				{
					return Json(false);
				}
				else
				{
					var xm = db.UserTable.Where(x => x.Password == Sellgm).FirstOrDefault();

					if (xm != null)
					{
						if (xm.Password == u.Password)
						{
							return Json(true);
						}
						else
						{
							return Json(false);
						}

					}
					else
					{
						return Json(true);

					}
				}
			}

		}

		[HttpPost]
		public IActionResult Edit_Post_Gmail(string Sellgm)
		{
			var u = UserController.user.FirstOrDefault();
			if (Sellgm == null)
			{
				return Json(false);
			}
			else
			{
				if (!Sellgm.Contains("@gmail.com"))
				{
					return Json(false);
				}
				else
				{
					var xm = db.UserTable.Where(x => x.Gmail == Sellgm).FirstOrDefault();

					if (xm != null)
					{
						if (xm.Gmail == u.Gmail)
						{
							return Json(true);
						}
						else
						{
							return Json(false);
						}
					}
					else
					{
						return Json(true);
					}
				}
			}

		}

		public IActionResult GetMsg()
		{

			var msg = db.MessageTable.OrderByDescending(x => x.Id).ToList();

			return Json(msg);
		}

		public IActionResult Master_Get_All_Message()
		{

			var message = db.MessageTable.OrderByDescending(x => x.Id).ToList();
			return Json(message);
		}

		public IActionResult Master_Get_Comment()
		{

			var comment = db.CommentTable.OrderByDescending(x => x.Id).ToList();
			return Json(comment);
		}

		[HttpPost]
		public IActionResult Delete_Conn(int ID)
		{
			var msg = db.MessageTable.Where(x => x.Id == ID).FirstOrDefault();
			db.MessageTable.Remove(msg);
			db.SaveChanges();
			var message = db.MessageTable.OrderByDescending(x => x.Id).ToList();

			return Json(message);
		}

		public IActionResult Get_User_Installment_Table()
		{
			var u = UserController.user.FirstOrDefault();
			var table = db.TrTable.Where(x => x.RoleID == u.RoleId && x.TransactionStatus == "Installments").ToList();
			return Json(table);
		}

		public IActionResult Get_Installment_Table(string RoleID)
		{
			var u = UserController.user.FirstOrDefault();
			var table = db.TrTable.Where(x => x.TransactionId == RoleID).FirstOrDefault();
			var Ins = db.InstallmentRecordTable.Where(x => x.Tr_ID == table.TransactionId).ToList();
			return Json(Ins);
		}

		[HttpPost]
		public IActionResult Installment_PY(int InstallmentID, CreditCard data)
		{
			if (data.number != "4242424242424242" || data.name == null || data.Cvc == null || data.ExpMonth == 0 || data.expYear == 0)
			{
				return Json(false);
			}

			var u = UserController.user.FirstOrDefault();
			var newuser = db.UserTable.Where(x => x.RoleId == u.RoleId).FirstOrDefault();
			var Installment_cart = db.InstallmentRecordTable.Where(x => x.Id == InstallmentID && x.User_ID == newuser.RoleId).FirstOrDefault();
			var table = db.TrTable.Where(x => x.TransactionId == Installment_cart.Tr_ID && x.RoleID == newuser.RoleId).FirstOrDefault();

			string[] str = table.CartList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			var cardid = 0;
			foreach (var i in str)
			{
				var c = db.CartTable.Where(x => x.Id == Convert.ToInt32(i) && x.MovieID == Installment_cart.Movie_ID && x.RoleID == newuser.RoleId && x.InstallmentLastTime != 0).FirstOrDefault();
				if (c != null)
				{
					cardid = c.Id;
				}
			}

			try
			{
				long amount = Convert.ToInt64(Installment_cart.Amount * 100);

				StripePayment stripePayment = new StripePayment(new CreditCard
				{
					Name = data.Name,
					Email = newuser.Gmail,
					AddressLine1 = newuser.Address,
					AddressLine2 = newuser.Address,
					AddressCity = newuser.City,
					AddressState = "True",
					AddressZip = newuser.AddressZip,//地址邮编
					Descripcion = $"Purchase on {DateTime.Now}",
					DetailsDescripcion = $"test {DateTime.Now:d}",
					Amount = amount, //2000 = 20.00   10000 == ??
					Currency = "MYR",
					Number = data.number,//4242 4242 4242 4242
					ExpMonth = data.ExpMonth,
					ExpYear = data.ExpYear,
					Cvc = data.Cvc //123
				}, stripeSecrets);
				Charge charge = stripePayment.ProccessPayment();

				var selectCard = db.CartTable.Where(x => x.RoleID == u.RoleId && x.Buy == false && x.Id == cardid).FirstOrDefault();
				bool check = true;

				if (selectCard.InstallmentTime != 0 && selectCard.IsInstallmentProduct == true && selectCard.InstallmentLastTime - 1 != 0)
				{

					Installment_cart.Status = "Installment Paid";
					Installment_cart.PaidTime = DateTime.Now.ToString();

					selectCard.InstallmentLastTime -= 1;
					selectCard.InstallmentLastBuy = true;
					selectCard.IsSelected = false;
					selectCard.Buy = false;

				}
				else if (selectCard.InstallmentTime != 0 && selectCard.IsInstallmentProduct == true && selectCard.InstallmentLastTime - 1 == 0)
				{
					//最后一次付款
					Installment_cart.Status = "Installment Paid";
					Installment_cart.PaidTime = DateTime.Now.ToString();
					db.SaveChanges();

					var LastfindInstallmentRecord = db.InstallmentRecordTable.Where(x => x.Status == "Pending" && x.Tr_ID == table.TransactionId && x.User_ID == newuser.RoleId).FirstOrDefault();
					if (LastfindInstallmentRecord == null)
					{
						var newtr = db.TrTable.Where(x => x.TransactionId == table.TransactionId).FirstOrDefault();
						newtr.TransactionStatus = "Paid";
						newtr.InstallmentStatus = true;
						newtr.PaidTime = DateTime.Now.ToString();
						db.SaveChanges();
					}

					selectCard.InstallmentTime = 0;
					selectCard.InstallmentLastTime = 0;
					selectCard.InstallmentLastBuy = false;
					selectCard.Buy = true;
					var p = db.MovieProductTable.Where(x => x.Movie_ID == selectCard.MovieID).FirstOrDefault();
					p.BuyCount += selectCard.LastQuantity;
					selectCard.LastQuantity = 0;

				}

				db.SaveChanges();

				try
				{
					string form = "wengchin1234567@gamil.com";
					MailMessage m = new MailMessage(form, newuser.Gmail);
					string date = DateTime.Now.ToString();

					LinkedResource LinkedImage = new LinkedResource(@".\007.png");
					LinkedImage.ContentId = "MyPic1";


					LinkedImage.ContentType = new System.Net.Mime.ContentType(MediaTypeNames.Image.Jpeg);


					AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
					   "<h1>Thank you for your purchase and support</h1>", null, "text/html"
					   );

					htmlView.LinkedResources.Add(LinkedImage);
					m.AlternateViews.Add(htmlView);

					m.Subject = "Thank You";
					m.BodyEncoding = Encoding.UTF8;
					m.IsBodyHtml = true;

					System.Net.Mail.SmtpClient c = new SmtpClient("smtp.gmail.com", 587);
					NetworkCredential n = new NetworkCredential("wengchin1234567@gmail.com", "ufznhwwqjoziiztw");

					c.EnableSsl = true;
					c.UseDefaultCredentials = false;
					c.Credentials = n;

					c.Send(m);

				}
				catch (Exception ex)
				{

				}






			}
			catch (Exception ex)
			{
				return Json(ex.Message);
			}

			return Json(true);
		}

		[HttpPost]
		public IActionResult Message_Post(string Title, string Message)
		{
			db.MessageTable.Add(new Message()
			{
				Title = Title,
				Msg = Message,
				CreatedDate = DateTime.Now.ToString()
			});
			db.SaveChanges();
			return Json(true);


		}

		public IActionResult Get_Master_Movie()
		{
			var mastermovie = db.MovieProductTable.Where(x => x.RoleId == "Master").ToList();
			return Json(mastermovie);
		}

		[HttpPost]
		public IActionResult Master_Del_Product(string Movie_ID)
		{
			var v = db.VideoTable.Where(x => x.Movie_ID == Movie_ID).FirstOrDefault();
			var m = db.MovieProductTable.Where(x => x.Movie_ID == Movie_ID).FirstOrDefault();

			db.VideoTable.Remove(v);
			db.SaveChanges();
			db.MovieProductTable.Remove(m);
			db.SaveChanges();
			return Json(true);
		}

		public IActionResult Get_Seller_Movie()
		{
			var sellermovie = db.MovieProductTable.Where(x => x.RoleId != "Master").ToList();
			return Json(sellermovie);
		}

		public IActionResult Get_Master_User()
		{
			var user = db.UserTable.Where(x => x.Role == "Customer").ToList();
			return Json(user);
		}

		[HttpPost]
		public IActionResult Master_Edit_Post_Password(string Sellgm, string TXp)
		{
			if (Sellgm == null)
			{
				return Json(false);
			}
			else
			{
				if (Sellgm.Length < 6)
				{
					return Json(false);
				}
				else
				{
					var xm = db.UserTable.Where(x => x.Password == Sellgm).FirstOrDefault();

					if (xm != null)
					{
						if (xm.Password == TXp)
						{
							return Json(true);
						}
						else
						{
							return Json(false);
						}

					}
					else
					{
						return Json(true);

					}
				}
			}

		}

		[HttpPost]
		public IActionResult Master_Edit_Post_Gmail(string Sellgm, string TXp)
		{
			if (Sellgm == null)
			{
				return Json(false);
			}
			else
			{
				if (!Sellgm.Contains("@gmail.com"))
				{
					return Json(false);
				}
				else
				{
					var xm = db.UserTable.Where(x => x.Gmail == Sellgm).FirstOrDefault();

					if (xm != null)
					{
						if (xm.Gmail == TXp)
						{
							return Json(true);
						}
						else
						{
							return Json(false);
						}
					}
					else
					{
						return Json(true);
					}
				}
			}

		}

		public IActionResult Get_Master_Seller()
		{
			var user = db.UserTable.Where(x => x.Role == "Seller").ToList();
			return Json(user);
		}

		[HttpPost]
		public IActionResult User_ForgotPassword(string myGmail, string myPassword)
		{
			var user = db.UserTable.Where(x => x.Gmail == myGmail).FirstOrDefault();
			var other = db.UserTable.Where(x => x.Password == myPassword).FirstOrDefault();
			if (user == null)
			{
				return Json("gmail");
			}

			if (other != null)
			{
				return Json(false);
			}

			user.Password = myPassword;
			db.SaveChanges();

			return Json(true);
		}

		public IActionResult Get_Master_Analysis_Product()
		{
			var pro = db.MovieProductTable.OrderByDescending(x => x.BuyCount).Take(10).ToList();
			return Json(pro);
		}

		public IActionResult Get_Master_Analysis_Product_Price()
		{

			var pro = db.MovieProductTable.OrderByDescending(x => x.MoviePrice).Take(10).ToList();
			return Json(pro);
		}

		public IActionResult Get_Master_Analysis_Seller()
		{

			var pro = db.MovieProductTable.Where(x => x.RoleId != "Master").ToList();
			var m1 = pro.GroupBy(x => x.RoleId).OrderByDescending(x => x.Sum((y => y.MoviePrice * y.BuyCount))).Take(5).ToList();

			return Json(m1);
		}

		public IActionResult Get_Master_Analysis_User()
		{
			var pro = db.TrTable.Where(x => x.TransactionStatus == "Paid" && x.InstallmentStatus == true).ToList();
			var n2 = pro.GroupBy(x => x.RoleID).OrderByDescending(x => x.Sum((y => y.Total + y.BeforeTotal))).Take(5).ToList();
			return Json(n2);
		}

	}
}
