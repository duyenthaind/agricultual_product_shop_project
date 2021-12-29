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
    public class OrderProductController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: OrderProduct
        public ActionResult Index()
        {
            var dh_order_product = db.dh_order_product.Include(d => d.dh_order).Include(d => d.dh_product);
            return View(dh_order_product.ToList());
        }

        // GET: OrderProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order_product dh_order_product = db.dh_order_product.Find(id);
            if (dh_order_product == null)
            {
                return HttpNotFound();
            }
            return View(dh_order_product);
        }

        // GET: OrderProduct/Create
        public ActionResult Create()
        {
            ViewBag.order_id = new SelectList(db.dh_order, "id", "address");
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name");
            return View();
        }

        // POST: OrderProduct/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,order_id,product_id,price,quantity")] dh_order_product dh_order_product)
        {
            if (ModelState.IsValid)
            {
                db.dh_order_product.Add(dh_order_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.order_id = new SelectList(db.dh_order, "id", "address", dh_order_product.order_id);
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dh_order_product.product_id);
            return View(dh_order_product);
        }

        // GET: OrderProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order_product dh_order_product = db.dh_order_product.Find(id);
            if (dh_order_product == null)
            {
                return HttpNotFound();
            }
            ViewBag.order_id = new SelectList(db.dh_order, "id", "address", dh_order_product.order_id);
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dh_order_product.product_id);
            return View(dh_order_product);
        }

        // POST: OrderProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,order_id,product_id,price,quantity")] dh_order_product dh_order_product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dh_order_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.order_id = new SelectList(db.dh_order, "id", "address", dh_order_product.order_id);
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dh_order_product.product_id);
            return View(dh_order_product);
        }

        // GET: OrderProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_order_product dh_order_product = db.dh_order_product.Find(id);
            if (dh_order_product == null)
            {
                return HttpNotFound();
            }
            return View(dh_order_product);
        }

        // POST: OrderProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_order_product dh_order_product = db.dh_order_product.Find(id);
            db.dh_order_product.Remove(dh_order_product);
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
