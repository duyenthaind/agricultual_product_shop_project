using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;
using log4net.Core;
using NongSanShop.Models;

namespace NongSanShop.Controllers
{
    public class ProductController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(ProductController));
        
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
            dh_product dhProduct = db.dh_product.Find(id);
            if (dhProduct == null)
            {
                return HttpNotFound();
            }
            return View(dhProduct);
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
        public ActionResult Create([Bind(Include = "id,name,description,price,quantity,category_id,avatar,created,updated")] dh_product dhProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dhProduct.avatar = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/uploads/products/" + FileName);
                        f.SaveAs(UploadPath);
                        dhProduct.avatar = FileName;
                    }

                    dhProduct.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                    db.dh_product.Add(dhProduct);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dhProduct);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                ViewBag.category_id = new SelectList(db.dh_category, "id", "name", dhProduct.category_id);
                return View(dhProduct);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_product dhProduct = db.dh_product.Find(id);
            if (dhProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.dh_category, "id", "name", dhProduct.category_id);
            return View(dhProduct);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,price,quantity,category_id,avatar,created,updated")] dh_product dhProduct)
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
                        dhProduct.avatar = FileName;
                    }

                    dhProduct.updated = DateTimeOffset.Now.ToUnixTimeSeconds();
                    db.Entry(dhProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dhProduct);
            }
            catch(Exception ex)
            {
                Logger.Error($"Edit product with id {dhProduct.id} error ", ex);
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                ViewBag.category_id = new SelectList(db.dh_category, "id", "name", dhProduct.category_id);
                return View(dhProduct);
            }


        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dh_product dhProduct = db.dh_product.Find(id);
            if (dhProduct == null)
            {
                return HttpNotFound();
            }
            return View(dhProduct);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_product dhProduct = db.dh_product.Find(id);
            if (dhProduct == null)
            {
                Logger.Info($"Find product with id {id} return null, redirect to index page");
                return RedirectToAction("Index");
            }
            try
            {
                db.dh_product.Remove(dhProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Logger.Error($"Delete product record {id} error ", ex);
                ViewBag.Error = "Không xoá bản ghi này!" + ex.Message;
                return View("Delete", dhProduct);
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
