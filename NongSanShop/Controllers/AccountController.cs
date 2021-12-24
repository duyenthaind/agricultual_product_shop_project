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
    public class AccountController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: Account
        public ActionResult Index()
        {
            return View(db.dh_user.ToList());
        }

        // GET: Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_user dh_user = db.dh_user.Find(id);
            if (dh_user == null)
            {
                return HttpNotFound();
            }
            return View(dh_user);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,username,password,name,email,phone,address,role,created,updated")] dh_user dh_user)
        {
            if (ModelState.IsValid)
            {
                db.dh_user.Add(dh_user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dh_user);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_user dh_user = db.dh_user.Find(id);
            if (dh_user == null)
            {
                return HttpNotFound();
            }
            return View(dh_user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,username,password,name,email,phone,address,role,created,updated")] dh_user dh_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dh_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dh_user);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_user dh_user = db.dh_user.Find(id);
            if (dh_user == null)
            {
                return HttpNotFound();
            }
            return View(dh_user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_user dh_user = db.dh_user.Find(id);
            db.dh_user.Remove(dh_user);
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
