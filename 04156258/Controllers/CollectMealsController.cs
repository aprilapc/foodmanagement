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
    public class CollectMealsController : Controller
    {
        //private AprilEntities db = new AprilEntities();
        private masterEntities db = new masterEntities();
        //private DB15Entities db = new DB15Entities();
        // GET: CollectMeals
        public ActionResult Index(int? id)
        {
            ViewBag.ID = Int32.Parse(Session["MemberID"].ToString());
            ViewBag.Name = Session["MemberName"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var memberid = Session["MemberID"];
            var collect = new List<CollectMeals>();
            foreach(var item in db.CollectMeals)
            {
                if (item.MemberID.Equals(memberid)){
                    collect.Add(item);
                }
            }
            return View(collect);
        }

        // GET: CollectMeals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectMeals collectMeals = db.CollectMeals.Find(id);
            if (collectMeals == null)
            {
                return HttpNotFound();
            }
            return View(collectMeals);
        }

        // GET: CollectMeals/Create
        public ActionResult Create()
        {
            ViewBag.MealsID = new SelectList(db.MealsAndDiscount, "MealsID", "MealsName");
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount");
            return View();
        }

        // POST: CollectMeals/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollectMealsID,CollectTime,MealsID,MemberID")] CollectMeals collectMeals)
        {
            if (ModelState.IsValid)
            {
                db.CollectMeals.Add(collectMeals);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MealsID = new SelectList(db.MealsAndDiscount, "MealsID", "MealsName", collectMeals.MealsID);
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", collectMeals.MemberID);
            return View(collectMeals);
        }

        // GET: CollectMeals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectMeals collectMeals = db.CollectMeals.Find(id);
            if (collectMeals == null)
            {
                return HttpNotFound();
            }
            ViewBag.MealsID = new SelectList(db.MealsAndDiscount, "MealsID", "MealsName", collectMeals.MealsID);
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", collectMeals.MemberID);
            return View(collectMeals);
        }

        // POST: CollectMeals/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollectMealsID,CollectTime,MealsID,MemberID")] CollectMeals collectMeals)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collectMeals).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MealsID = new SelectList(db.MealsAndDiscount, "MealsID", "MealsName", collectMeals.MealsID);
            ViewBag.MemberID = new SelectList(db.Member, "MemberID", "MemberAccount", collectMeals.MemberID);
            return View(collectMeals);
        }

        // GET: CollectMeals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectMeals collectMeals = db.CollectMeals.Find(id);
            if (collectMeals == null)
            {
                return HttpNotFound();
            }
            return View(collectMeals);
        }

        // POST: CollectMeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CollectMeals collectMeals = db.CollectMeals.Find(id);
            db.CollectMeals.Remove(collectMeals);
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
