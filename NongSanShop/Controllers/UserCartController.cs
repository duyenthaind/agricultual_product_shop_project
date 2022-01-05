using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NongSanShop.Filters;
using NongSanShop.Models;

namespace NongSanShop.Controllers
{
    [UserAuthorizationFilter]
    public class UserCartController : Controller
    {
        private NongSanDB db = new NongSanDB();



        // GET: UserCart
        public ActionResult Index()
        {
            var dh_cart = db.dh_cart.Include(d => d.dh_product).Include(d => d.dh_user);
            Session["no_cart"] = dh_cart.ToList().Count();
            return View(dh_cart.ToList());
        }

        // GET: UserCart/Details/5
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_cart dh_cart = db.dh_cart.Find(id);
            if (dh_cart == null)
            {
                return HttpNotFound();
            }
            return View(dh_cart);
        }*/

        // GET: UserCart/Create
        /*public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name");
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username");
            return View();
        }*/

        // POST: UserCart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create(int productId, int userId)
        {
            var product = db.dh_product.FirstOrDefault(p => p.id == productId);
            if (product == null)
            {
                return HttpNotFound();
            }
            var cartEmpty = db.dh_cart.FirstOrDefault(c => c.product_id == productId&&c.user_id==userId);
            if (cartEmpty == null)
            {
                dh_cart cartItem = new dh_cart();
                cartItem.price = product.price;
                cartItem.quantity = 1;
                cartItem.product_id = productId;
                cartItem.user_id = userId;
                db.dh_cart.Add(cartItem);
            }
            else
            {
               cartEmpty.quantity += 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: UserCart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_cart dh_cart = db.dh_cart.Find(id);
            if (dh_cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dh_cart.product_id);
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dh_cart.user_id);
            return View(dh_cart);
        }

        // POST: UserCart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,quantity,price,product_id,created,updated")] dh_cart dh_cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dh_cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.dh_product, "id", "name", dh_cart.product_id);
            ViewBag.user_id = new SelectList(db.dh_user, "id", "username", dh_cart.user_id);
            return View(dh_cart);
        }

        // GET: UserCart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_cart dh_cart = db.dh_cart.Find(id);
            if (dh_cart == null)
            {
                return HttpNotFound();
            }
            return View(dh_cart);
        }

        // POST: UserCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_cart dh_cart = db.dh_cart.Find(id);
            db.dh_cart.Remove(dh_cart);
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
