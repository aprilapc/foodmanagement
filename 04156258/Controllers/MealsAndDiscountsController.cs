using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _04156258.Models;

namespace _04156258.Controllers
{
    public class MealsAndDiscountsController : Controller
    {
        private DB15Entities db = new DB15Entities();
        //
        public ActionResult Index(string sort)
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            Session["sort"] = sort;
            if (sort == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var meals = new List<MealsAndDiscount>();
            foreach (var item in db.MealsAndDiscount)
            {
                if (item.MealsSort.Equals(sort))
                {
                    meals.Add(item);
                }
            }
            return View(meals);
        }
        public ActionResult Feedback(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            ViewBag.Sort = Session["sort"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount fm = db.MealsAndDiscount.Find(id);
            if (fm == null)
            {
                return HttpNotFound();
            }
            Session["MID"] = fm.MealsID;
            Satisfaction fd = new Satisfaction();
            fd.MealsID = fm.MealsID;
            fd.MemberID = Int32.Parse(Session["MemberID"].ToString());
            fd.MessageContent = "";
            fd.Score = 0;
            return View(fd);
        }
        [HttpPost]
        public ActionResult Feedback([Bind(Include = "MessageContent,Score")] Satisfaction satisfaction)
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            string sort = Session["sort"].ToString();
            ViewBag.Sort = sort;
            if (ModelState.IsValid)
            {
                satisfaction.MemberID = Int32.Parse(Session["MemberID"].ToString());
                satisfaction.MealsID = Int32.Parse(Session["MID"].ToString());
                db.Satisfaction.Add(satisfaction);
                db.SaveChanges();
                TempData["successful"] = "<script>alert('感謝您提供餐點評價');</script>";
                return RedirectToAction("AllFeedback", new { id = satisfaction.MealsID });
            }
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", satisfaction.MemberID);
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", satisfaction.MealsID);
            return View(satisfaction);
        }
        public ActionResult AllFeedback(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            ViewBag.FBid = id;
            ViewBag.Sort = Session["sort"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mealsfeedback = new List<Satisfaction>();
            foreach (var item in db.Satisfaction)
            {
                if (item.MealsID.Equals(id))
                {
                    mealsfeedback.Add(item);
                }
            }
            return View(mealsfeedback);
        }
        public ActionResult Remind()
        {
            List < MealsAndDiscountsController > mealsanddiscounts;
            int id = 1;
            foreach (var item in db.meals)
            {
                mealsanddiscounts = db.MealsAndDiscounts.Select(m => m.MealsID == id).ToList();
            }
            int count = mealsanddiscounts.Count();
            ViewBag.Count = count;
            return View();
        }
        //public ActionResult Index()
        //{
        //    ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
        //    ViewBag.Name = Session["MemberName"];
        //    return View();
        //}

        // GET: MealsAndDiscounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount mealsAndDiscount = db.MealsAndDiscount.Find(id);
            if (mealsAndDiscount == null)
            {
                return HttpNotFound();
            }
            return View(mealsAndDiscount);
        }

        // GET: MealsAndDiscounts/Create
        public ActionResult Create()
        {
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount");
            return View();
        }

        // POST: MealsAndDiscounts/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MealsID,MealsName,MealsPrice,MealsSort,MealsDiscount,RestaurantID")] MealsAndDiscount mealsAndDiscount)
        {
            if (ModelState.IsValid)
            {
                db.MealsAndDiscount.Add(mealsAndDiscount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", mealsAndDiscount.RestaurantID);
            return View(mealsAndDiscount);
        }

        // GET: MealsAndDiscounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount mealsAndDiscount = db.MealsAndDiscount.Find(id);
            if (mealsAndDiscount == null)
            {
                return HttpNotFound();
            }
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", mealsAndDiscount.RestaurantID);
            return View(mealsAndDiscount);
        }

        // POST: MealsAndDiscounts/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MealsID,MealsName,MealsPrice,MealsSort,MealsDiscount,RestaurantID")] MealsAndDiscount mealsAndDiscount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mealsAndDiscount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", mealsAndDiscount.RestaurantID);
            return View(mealsAndDiscount);
        }

        // GET: MealsAndDiscounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealsAndDiscount mealsAndDiscount = db.MealsAndDiscount.Find(id);
            if (mealsAndDiscount == null)
            {
                return HttpNotFound();
            }
            return View(mealsAndDiscount);
        }

        // POST: MealsAndDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MealsAndDiscount mealsAndDiscount = db.MealsAndDiscount.Find(id);
            db.MealsAndDiscount.Remove(mealsAndDiscount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
