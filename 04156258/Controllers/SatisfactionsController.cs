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
    public class SatisfactionsController : Controller
    {
        //private masterEntities db = new masterEntities();
        private DB15Entities db = new DB15Entities();

        public ActionResult Index()
        {
            var satisfaction = db.Satisfaction.Include(s => s.Member).Include(s => s.MealsID);
            return View(satisfaction.ToList());
        }

        // GET: Satisfactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satisfaction satisfaction = db.Satisfaction.Find(id);
            if (satisfaction == null)
            {
                return HttpNotFound();
            }
            return View(satisfaction);
        }

        // GET: Satisfactions/Create
        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount");
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount");
            return View();
        }

        // POST: Satisfactions/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,MessageContent,Score,RestaurantID,MemberID")] Satisfaction satisfaction)
        {
            if (ModelState.IsValid)
            {
                db.Satisfaction.Add(satisfaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", satisfaction.MemberID);
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", satisfaction.MealsID);
            return View(satisfaction);
        }

        // GET: Satisfactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satisfaction satisfaction = db.Satisfaction.Find(id);
            if (satisfaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", satisfaction.MemberID);
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", satisfaction.MealsID);
            return View(satisfaction);
        }

        // POST: Satisfactions/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,MessageContent,Score,RestaurantID,MemberID")] Satisfaction satisfaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(satisfaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", satisfaction.MemberID);
            ViewBag.RestaurantID = new SelectList(db.Restaurant, "RestaurantID", "RestaurantAccount", satisfaction.MealsID);
            return View(satisfaction);
        }

        // GET: Satisfactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Satisfaction satisfaction = db.Satisfaction.Find(id);
            if (satisfaction == null)
            {
                return HttpNotFound();
            }
            return View(satisfaction);
        }

        // POST: Satisfactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Satisfaction satisfaction = db.Satisfaction.Find(id);
            db.Satisfaction.Remove(satisfaction);
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
