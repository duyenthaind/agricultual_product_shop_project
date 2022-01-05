using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;
using NongSanShop.Filters;
using NongSanShop.Models;
using PagedList;

namespace NongSanShop.Controllers
{
    [AdminAuthorizationFilter]
    public class CartController : Controller
    {

        private static readonly ILog Logger = LogManager.GetLogger(nameof(CartController));
        
        private NongSanDB db = new NongSanDB();

        // GET: Cart
        public ActionResult Index(int? page)
        {
            var dh_cart = db.dh_cart.Include(d => d.dh_product).Include(d => d.dh_user);
            dh_cart = dh_cart.OrderBy(d => d.id);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(dh_cart.ToPagedList(pageSize,pageNumber));
        }

        // GET: Cart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_cart dhCart = db.dh_cart.Find(id);
            if (dhCart == null)
            {
                return HttpNotFound();
            }
            return View(dhCart);
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name");
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,quantity,price,product_id,created,updated")] dh_cart dhCart)
        {
            if (ModelState.IsValid)
            {
                dhCart.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                db.dh_cart.Add(dhCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dhCart.product_id);
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dhCart.user_id);
            return View(dhCart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_cart dhCart = db.dh_cart.Find(id);
            if (dhCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dhCart.product_id);
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dhCart.user_id);
            return View(dhCart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,quantity,price,product_id,created,updated")] dh_cart dhCart)
        {
            if (ModelState.IsValid)
            {
                dhCart.updated = DateTimeOffset.Now.ToUnixTimeSeconds();
                db.Entry(dhCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dhCart.product_id);
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dhCart.user_id);
            return View(dhCart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_cart dhCart = db.dh_cart.Find(id);
            if (dhCart == null)
            {
                return HttpNotFound();
            }
            return View(dhCart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_cart dhCart = db.dh_cart.Find(id);
            if (dhCart == null)
            {
                Logger.Info($"Find cart id {id} return empty result ");
                return RedirectToAction("Index");    
            }
            db.dh_cart.Remove(dhCart);
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
