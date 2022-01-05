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
    public class CategoryController : Controller
    {

        private static readonly ILog Logger = LogManager.GetLogger(nameof(CategoryController));
        
        private NongSanDB db = new NongSanDB();

        // GET: Category
        public ActionResult Index(int? page)
        {
            var dh_category = db.dh_category.Select(d => d);
            dh_category = dh_category.OrderBy(d => d.id);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(dh_category.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Display(int? page)
        {
            var dh_category = db.dh_category.Select(d=>d);
            dh_category = dh_category.OrderBy(d => d.id);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(dh_category.ToPagedList(pageNumber, pageSize));
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_category dhCategory = db.dh_category.Find(id);
            if (dhCategory == null)
            {
                return HttpNotFound();
            }
            return View(dhCategory);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,created,updated,avatar")] dh_category dhCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dhCategory.avatar = "";
                    dhCategory.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/uploads/categories/" + FileName);
                        f.SaveAs(UploadPath);
                        dhCategory.avatar = FileName;
                    }
                    db.dh_category.Add(dhCategory);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                return View(dhCategory);
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_category dhCategory = db.dh_category.Find(id);
            if (dhCategory == null)
            {
                return HttpNotFound();
            }
            return View(dhCategory);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,created,updated,avatar")] dh_category dhCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dhCategory.updated = DateTimeOffset.Now.ToUnixTimeSeconds();
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/uploads/categories/" + FileName);
                        f.SaveAs(UploadPath);
                        dhCategory.avatar = FileName;
                    }
                    db.Entry(dhCategory).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                return View(dhCategory);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_category dhCategory = db.dh_category.Find(id);
            if (dhCategory == null)
            {
                return HttpNotFound();
            }
            return View(dhCategory);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_category dhCategory = db.dh_category.Find(id);
            if (dhCategory == null)
            {
                Logger.Info($"Find category with id {id} return result null, redirect to index page ");
                return RedirectToAction("Index");
            }
            try
            {
                db.dh_category.Remove(dhCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error($"Delete record {id} error ", ex);
                ViewBag.Error = "Không được xóa bản ghi này" + ex.Message;
                return View("Delete", dhCategory);
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
