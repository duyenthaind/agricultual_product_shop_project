using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NongSanShop.Models;

namespace NongSanShop.Controllers
{
    public class OrderController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: Order
        public ActionResult Index()
        {
            var dh_order = db.dh_order.Include(d => d.dh_user);
            return View(dh_order.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order dh_order = db.dh_order.Find(id);
            if (dh_order == null)
            {
                return HttpNotFound();
            }
            return View(dh_order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,address,name,email,code_name,status,created,updated")] dh_order dh_order)
        {
            if (ModelState.IsValid)
            {
                db.dh_order.Add(dh_order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dh_order.user_id);
            return View(dh_order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order dh_order = db.dh_order.Find(id);
            if (dh_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dh_order.user_id);
            return View(dh_order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,address,name,email,code_name,status,created,updated")] dh_order dh_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dh_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dh_order.user_id);
            return View(dh_order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order dh_order = db.dh_order.Find(id);
            if (dh_order == null)
            {
                return HttpNotFound();
            }
            return View(dh_order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_order dh_order = db.dh_order.Find(id);
            db.dh_order.Remove(dh_order);
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
