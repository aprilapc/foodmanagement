using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _04156258.Models;
using System.Web.Security;
using System.Net;
using System.Data.Entity;

namespace _04156258.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private AprilEntities DB = new AprilEntities();
        //private masterEntities DB = new masterEntities();
        private DB15Entities DB = new DB15Entities();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Loginchoose()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include="MemberAccount,MemberPassword")] Member member)
        {
            Member memberfind = DB.Member.SingleOrDefault(m => m.MemberAccount == member.MemberAccount);
            if(memberfind != null)
            {
                Session["MemberID"] = memberfind.MemberID;
                Session["MemberName"] = memberfind.MemberName;
                if(memberfind.MemberPassword == member.MemberPassword)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, member.MemberAccount, DateTime.Now, DateTime.Now.AddMinutes(100), true, member.MemberAccount, FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("LoginIndex");
                }
                else
                {
                    TempData["null"] = "<script>alert('登入失敗,請確認是否註冊或帳號密碼輸入錯誤');</script>";
                    return View();
                }
            }
            else
            {
                TempData["null"] = "<script>alert('此帳號尚未註冊,請先成為會員');</script>";
                return RedirectToAction("Register");
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult LoginIndex()
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            var mealsanddiscount = DB.MealsAndDiscount;
            List<MealsAndDiscount> meals = new List<MealsAndDiscount>();
            
            foreach(var item in mealsanddiscount)
            {
                while (meals.Count == 0)
                {
                    meals.Add(item);
                }
                int count = 0;
                foreach (var same in meals)
                {
                    if (item.MealsSort.Equals(same.MealsSort))
                    {
                    }
                    else
                    {
                        count++;
                    }
                }
                if (count == meals.Count())
                {
                    meals.Add(item);
                }
            }
            SelectList data = new SelectList(meals, "MealsSort", "MealsSort");
           
            ViewBag.sort = data;

            return View();
        }
        [AllowAnonymous]
        public ActionResult Registerchoose()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include ="MemberAccount,MemberPassword,MemberName,MemberEmail")]Member member)
        {
            Member memberfind = DB.Member.SingleOrDefault(m => m.MemberAccount == member.MemberAccount);
            if (memberfind != null)
            {
                TempData["exist"] = "<script>alert('此帳號已註冊,請嘗試登入');</script>";
                return RedirectToAction("Login");
            }
            else if (ModelState.IsValid)
            {
                DB.Member.Add(member);
                DB.SaveChanges();
                Session["MemberID"] = member.MemberID;
                Session["MemberName"] = member.MemberName;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, member.MemberAccount, DateTime.Now, DateTime.Now.AddMinutes(100), true, member.MemberAccount, FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("LoginIndex");
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult RestaurantLogin()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestaurantLogin([Bind(Include = "RestaurantAccount,RestaurantPassword")] Restaurant restaurant)
        {
            Restaurant restaurantfind = DB.Restaurant.SingleOrDefault(m => m.RestaurantAccount == restaurant.RestaurantAccount);
            
            if (restaurantfind != null)
            {
                Session["RestaurantID"] = restaurantfind.RestaurantID;
                Session["RestaurantName"] = restaurantfind.RestaurantName;
                int id = restaurantfind.RestaurantID;
                if (restaurantfind.RestaurantPassword == restaurant.RestaurantPassword)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, restaurant.RestaurantAccount, DateTime.Now, DateTime.Now.AddMinutes(100), true, restaurant.RestaurantAccount, FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("RestaurantLoginIndex","Home", new { id = id });
                }
                else
                {
                    TempData["null"] = "<script>alert('登入失敗,請確認是否註冊或帳號密碼輸入錯誤');</script>";
                    return View();
                }
            }
            else
            {
                TempData["null"] = "<script>alert('此帳號尚未註冊,請先成為會員');</script>";
                return RedirectToAction("RestaurantRegister");
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult RestaurantLoginIndex(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var restaurantid = Session["RestaurantID"];
            var meals = new List<MealsAndDiscount>();
            foreach (var item in DB.MealsAndDiscount)
            {
                if (item.RestaurantID.Equals(restaurantid))
                {
                    meals.Add(item);
                }
            }
            return View(meals);
        }
        [AllowAnonymous]
        public ActionResult RestaurantRegister()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestaurantRegister([Bind(Include = "RestaurantAccount,RestaurantPassword,RestaurantName,RestaurantPhone,RestaurantAddress,RestaurantOpentime")]Restaurant restaurant)
        {
            Restaurant restaurantfind = DB.Restaurant.SingleOrDefault(m => m.RestaurantAccount == restaurant.RestaurantAccount);
            if (restaurantfind != null)
            {
                TempData["exist"] = "<script>alert('此帳號已註冊,請嘗試登入');</script>";
                return RedirectToAction("RestaurantLogin");
            }
            else if (ModelState.IsValid)
            {
                DB.Restaurant.Add(restaurant);
                DB.SaveChanges();
                Session["RestaurantID"] = restaurant.RestaurantID;
                Session["RestaurantName"] = restaurant.RestaurantName;
                int id = restaurant.RestaurantID;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, restaurant.RestaurantAccount, DateTime.Now, DateTime.Now.AddMinutes(100), true, restaurant.RestaurantAccount, FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("RestaurantLoginIndex","Home",new { id = id });
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Ownspace(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = DB.Member.Find(id);
            if(member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        [AllowAnonymous]
        public ActionResult RestaurantOwnspace(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = DB.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }
        [AllowAnonymous]
        public ActionResult MealsEdit(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount mealsAndDiscount = DB.MealsAndDiscount.Find(id);
            if (mealsAndDiscount == null)
            {
                return HttpNotFound();
            }
            return View(mealsAndDiscount);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MealsEdit([Bind(Include = "MealsID,MealsName,MealsPrice,MealsSort,MealsDiscount,RestaurantID")] MealsAndDiscount mealsAndDiscount)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(mealsAndDiscount).State = EntityState.Modified;
                DB.SaveChanges();
                int id = mealsAndDiscount.RestaurantID;
                return RedirectToAction("RestaurantLoginIndex",new { id = id });
            }
            return View(mealsAndDiscount);
        }

        [AllowAnonymous]
        public ActionResult MealsCreate()
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MealsCreate([Bind(Include = "MealsID,MealsName,MealsPrice,MealsSort,MealsDiscount,RestaurantID")] MealsAndDiscount mealsAndDiscount)
        {
            if (ModelState.IsValid)
            {
                mealsAndDiscount.RestaurantID = Int32.Parse(Session["RestaurantID"].ToString());
                DB.MealsAndDiscount.Add(mealsAndDiscount);
                DB.SaveChanges();
                int id = mealsAndDiscount.RestaurantID;
                return RedirectToAction("RestaurantLoginIndex",new { id = id });
            }
            return View(mealsAndDiscount);
        }
        [AllowAnonymous]
        public ActionResult MealsDetails(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount mealsAndDiscount = DB.MealsAndDiscount.Find(id);
            if (mealsAndDiscount == null)
            {
                return HttpNotFound();
            }
            return View(mealsAndDiscount);
        }

        [AllowAnonymous]
        public ActionResult MealsDelete(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount mealsAndDiscount = DB.MealsAndDiscount.Find(id);
            if (mealsAndDiscount == null)
            {
                return HttpNotFound();
            }
            return View(mealsAndDiscount);
        }

        [AllowAnonymous]
        [HttpPost, ActionName("MealsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult MealsDeleteConfirmed(int id)
        {
            MealsAndDiscount mealsAndDiscount = DB.MealsAndDiscount.Find(id);
            DB.MealsAndDiscount.Remove(mealsAndDiscount);
            DB.SaveChanges();
            int Rid = Int32.Parse(Session["RestaurantID"].ToString());
            return RedirectToAction("RestaurantLoginIndex", new { id = Rid });
        }

        [AllowAnonymous]
        public ActionResult MealsFeedback(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["RestaurantID"].ToString());
            ViewBag.Name = Session["RestaurantName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feedback = new List<Satisfaction>();
            foreach (var item in DB.Satisfaction)
            {
                if (item.MealsID.Equals(id))
                {
                    feedback.Add(item);
                }
            }
            return View(feedback);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
    }
}