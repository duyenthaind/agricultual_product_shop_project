using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;
using NongSanShop.Models;

namespace NongSanShop.Controllers
{
    public class OrderController : Controller
    {

        private static readonly ILog Logger = LogManager.GetLogger(nameof(OrderController));
        
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
            dh_order dhOrder = db.dh_order.Find(id);
            if (dhOrder == null)
            {
                return HttpNotFound();
            }
            return View(dhOrder);
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
        public ActionResult Create([Bind(Include = "id,user_id,address,name,email,code_name,status,created,updated")] dh_order dhOrder)
        {
            if (ModelState.IsValid)
            {
                dhOrder.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                db.dh_order.Add(dhOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dhOrder.user_id);
            return View(dhOrder);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order dhOrder = db.dh_order.Find(id);
            if (dhOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dhOrder.user_id);
            return View(dhOrder);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,address,name,email,code_name,status,created,updated")] dh_order dhOrder)
        {
            if (ModelState.IsValid)
            {
                dhOrder.updated = DateTimeOffset.Now.ToUnixTimeSeconds();
                db.Entry(dhOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dhOrder.user_id);
            return View(dhOrder);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order dhOrder = db.dh_order.Find(id);
            if (dhOrder == null)
            {
                return HttpNotFound();
            }
            return View(dhOrder);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_order dhOrder = db.dh_order.Find(id);
            if (dhOrder == null)
            {
                Logger.Info($"Find order with id {id} return result null, redirect to index page");
                return RedirectToAction("Index");
            }
            db.dh_order.Remove(dhOrder);
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
