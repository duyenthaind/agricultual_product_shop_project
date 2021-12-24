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
    public class CategoryController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: Category
        public ActionResult Index()
        {
            return View(db.dh_category.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_category dh_category = db.dh_category.Find(id);
            if (dh_category == null)
            {
                return HttpNotFound();
            }
            return View(dh_category);
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
        public ActionResult Create([Bind(Include = "id,name,description,created,updated,avatar")] dh_category dh_category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dh_category.avatar = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/uploads/categories/" + FileName);
                        f.SaveAs(UploadPath);
                        dh_category.avatar = FileName;
                    }
                    db.dh_category.Add(dh_category);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                return View(dh_category);
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_category dh_category = db.dh_category.Find(id);
            if (dh_category == null)
            {
                return HttpNotFound();
            }
            return View(dh_category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,created,updated,avatar")] dh_category dh_category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dh_category).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                return View(dh_category);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_category dh_category = db.dh_category.Find(id);
            if (dh_category == null)
            {
                return HttpNotFound();
            }
            return View(dh_category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_category dh_category = db.dh_category.Find(id);
            try
            {
                db.dh_category.Remove(dh_category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không được xóa bản ghi này" + ex.Message;
                return View("Delete", dh_category);
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
