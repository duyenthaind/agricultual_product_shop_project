﻿using System;
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
    public class CartController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: Cart
        public ActionResult Index()
        {
            var dh_cart = db.dh_cart.Include(d => d.dh_product).Include(d => d.dh_user);
            return View(dh_cart.ToList());
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
