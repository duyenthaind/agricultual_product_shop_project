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
    public class BlogController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: Blog
        public ActionResult Index()
        {
            return View(db.dh_blog.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_blog dh_blog = db.dh_blog.Find(id);
            if (dh_blog == null)
            {
                return HttpNotFound();
            }
            return View(dh_blog);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,thumbnail,content,created,updated")] dh_blog dh_blog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.dh_blog.Add(dh_blog);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dh_blog);
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                return View(dh_blog);
            }


        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_blog dh_blog = db.dh_blog.Find(id);
            if (dh_blog == null)
            {
                return HttpNotFound();
            }
            return View(dh_blog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,thumbnail,content,created,updated")] dh_blog dh_blog)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dh_blog).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dh_blog);
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                return View(dh_blog);
            }

        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_blog dh_blog = db.dh_blog.Find(id);
            if (dh_blog == null)
            {
                return HttpNotFound();
            }
            return View(dh_blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_blog dh_blog = db.dh_blog.Find(id);
            try
            {
                db.dh_blog.Remove(dh_blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Không xoá bản ghi này!" + ex.Message;
                return View("Delete", dh_blog);
            }

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
