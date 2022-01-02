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

namespace NongSanShop.Controllers
{
    [AdminAuthorizationFilter]
    public class BlogController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(BlogController));

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

            dh_blog dhBlog = db.dh_blog.Find(id);
            if (dhBlog == null)
            {
                return HttpNotFound();
            }

            return View(dhBlog);
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
        public ActionResult Create([Bind(Include = "id,thumbnail,content,created,updated")] dh_blog dhBlog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dhBlog.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                    db.dh_blog.Add(dhBlog);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(dhBlog);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                Logger.Error("Create new blog error, ", ex);
                return View(dhBlog);
            }
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dh_blog dhBlog = db.dh_blog.Find(id);
            if (dhBlog == null)
            {
                return HttpNotFound();
            }

            return View(dhBlog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,thumbnail,content,created,updated")] dh_blog dhBlog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dhBlog.updated = DateTimeOffset.Now.ToUnixTimeSeconds();
                    db.Entry(dhBlog).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(dhBlog);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                Logger.Error($"Edit blog with id {dhBlog.id} error", ex);
                return View(dhBlog);
            }
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dh_blog dhBlog = db.dh_blog.Find(id);
            if (dhBlog == null)
            {
                return HttpNotFound();
            }

            return View(dhBlog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_blog dhBlog = db.dh_blog.Find(id);
            if (dhBlog == null)
            {
                Logger.Info($"Find record blog with id {id}, result not found, route user to controller home page");
                return RedirectToAction("Index");
            }

            try
            {
                db.dh_blog.Remove(dhBlog);
                db.SaveChanges();
                Logger.Info($"Deleted blog with id : {id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error($"Delete log record id {id} error", ex);
                ViewBag.Error = "Không xoá bản ghi này!" + ex.Message;
                return View("Delete", dhBlog);
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