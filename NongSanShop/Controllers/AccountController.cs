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
using NongSanShop.Util;
using PagedList;

namespace NongSanShop.Controllers
{
    [AdminAuthorizationFilter]
    public class AccountController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(AccountController));

        private NongSanDB db = new NongSanDB();

        // GET: Account
        public ActionResult Index(int? page)
        {
            var account = db.dh_user.Select(a => a);
            account = account.OrderBy(a => a.id);
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(account.ToPagedList(pageNumber,pageSize));
        }

        // GET: Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dh_user dhUser = db.dh_user.Find(id);
            if (dhUser == null)
            {
                return HttpNotFound();
            }

            return View(dhUser);
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
        public ActionResult Create(
            [Bind(Include = "id,username,password,name,email,phone,address,role,created,updated")] dh_user dhUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hashedPassword = HMac256Helper.HashPassword(dhUser.password, dhUser.username);
                    dhUser.password = hashedPassword;
                    dhUser.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                    db.dh_user.Add(dhUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(dhUser);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                Logger.Error("Create new account error", ex);
                return View(dhUser);
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dh_user dhUser = db.dh_user.Find(id);
            if (dhUser == null)
            {
                return HttpNotFound();
            }

            return View(dhUser);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "id,username,password,name,email,phone,address,role,created,updated")] dh_user dhUser)
        {
            try
            {
                var oldPassword = db.dh_user.Where(p => p.id == dhUser.id)
                    .Select(p => p.password).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    if (!dhUser.password.Equals(oldPassword))
                    {
                        var hashedPassword = HMac256Helper.HashPassword(dhUser.password, dhUser.username);
                        dhUser.password = hashedPassword;
                    }

                    dhUser.updated = DateTimeOffset.Now.ToUnixTimeSeconds();
                    db.Entry(dhUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(dhUser);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập liệu" + ex.Message;
                Logger.Error($"Edit account {dhUser.id} error ", ex);
                return View(dhUser);
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dh_user dhUser = db.dh_user.Find(id);
            if (dhUser == null)
            {
                return HttpNotFound();
            }

            return View(dhUser);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dh_user dhUser = db.dh_user.Find(id);
            if (dhUser == null)
            {
                Logger.Info($"Find account {id} return result null, redirect to index page");
                return RedirectToAction("Index");
            }

            try
            {
                db.dh_user.Remove(dhUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không được xóa bản ghi này" + ex.Message;
                return View("Delete", dhUser);
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