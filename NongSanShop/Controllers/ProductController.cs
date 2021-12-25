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
    public class ProductController : Controller
    {
        private NongSanDB db = new NongSanDB();

        // GET: Product
        public ActionResult Index()
        {
            var dh_product = db.dh_product.Include(d => d.dh_category);
            return View(dh_product.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_product dh_product = db.dh_product.Find(id);
            if (dh_product == null)
            {
                return HttpNotFound();
            }
            return View(dh_product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.dh_category, "id", "name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,price,quantity,category_id,avatar,created,updated")] dh_product dh_product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dh_product.avatar = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/uploads/products/" + FileName);
                        f.SaveAs(UploadPath);
                        dh_product.avatar = FileName;
                    }
                    db.dh_product.Add(dh_product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dh_product);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                ViewBag.category_id = new SelectList(db.dh_category, "id", "name", dh_product.category_id);
                return View(dh_product);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_product dh_product = db.dh_product.Find(id);
            if (dh_product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.dh_category, "id", "name", dh_product.category_id);
            return View(dh_product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,price,quantity,category_id,avatar,created,updated")] dh_product dh_product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/uploads/products/" + FileName);
                        f.SaveAs(UploadPath);
                        dh_product.avatar = FileName;
                    }
                    db.Entry(dh_product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dh_product);
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                ViewBag.category_id = new SelectList(db.dh_category, "id", "name", dh_product.category_id);
                return View(dh_product);
            }


        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_product dh_product = db.dh_product.Find(id);
            if (dh_product == null)
            {
                return HttpNotFound();
            }
            return View(dh_product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_product dh_product = db.dh_product.Find(id);
            try
            {
                db.dh_product.Remove(dh_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Không xoá bản ghi này!" + ex.Message;
                return View("Delete", dh_product);
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
