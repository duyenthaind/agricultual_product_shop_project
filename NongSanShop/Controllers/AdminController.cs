using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using NongSanShop.Common;
using NongSanShop.Configuration.Attribute;
using NongSanShop.CustomException;
using NongSanShop.Filters;
using NongSanShop.Models;
using NongSanShop.Models.Custom.Builder;
using NongSanShop.Util;
using NongSanShop.Models.Custom;
using System.Data.Entity;

namespace NongSanShop.Controllers
{
    [AdminAuthorizationFilter]
    public class AdminController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(AdminController));

        private readonly NongSanDB dbContext = new NongSanDB();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            dh_user info = null;
            if (Session[Constants.SessionItem.User] != null)
            {
                string userName = ((AppUser) Session[Constants.SessionItem.User]).Username.ToString();
                info = dbContext.dh_user.Single(e => e.username == userName);
            }

            return View(info);
        }


        [HttpPost]
        public ActionResult Info([Bind(Include = "id,name,email,address,phone")] dh_user dhUser)
        {
            try
            {
                var currentUser = dbContext.dh_user.Find(dhUser.id);
                if (currentUser == null)
                {
                    return HttpNotFound();
                }

                currentUser.name = dhUser.name;
                currentUser.email = dhUser.email;
                currentUser.address = dhUser.address;
                currentUser.phone = dhUser.phone;
                dbContext.Entry(currentUser).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Info");
            }
            catch (Exception ex)
            {
                Logger.Error("Alter user details error", ex);
            }

            return View(dhUser);
        }

        [SkipAction]
        public ActionResult Login()
        {
            return View();
        }

        [SkipAction]
        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            try
            {
                var username = formCollection["username"];
                var rawPassword = formCollection["password"];

                var dhUser = dbContext.dh_user.FirstOrDefault(p => p.username == username);
                if (dhUser == null)
                {
                    throw new AuthorizationException("User is not found");
                }

                var isAuthenticationOk = HMac256Helper.ProcessAuthentication(rawPassword, username, dhUser.password);

                if (isAuthenticationOk.Equals(false))
                {
                    throw new AuthorizationException("Password is not correct");
                }

                var appUser = new AppUserBuilder()
                    .withUserId(dhUser.id)
                    .withUserName(username)
                    .withAppRole(dhUser.role.GetValueOrDefault())
                    .build();
                if (appUser.AppRole == null)
                {
                    throw new AuthorizationException("User cannot be authorized, role is something malformed");
                }

                if (appUser.AppRole.IsAdmin.Equals(false))
                {
                    throw new AuthorizationException("You have no authority to access this site");
                }


                Session[Constants.SessionItem.User] = appUser;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error when login ", ex);
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Login");
            }
        }

        [SkipAction]
        [HttpPost]
        public string Test(string password, string username)
        {
            return HMac256Helper.HashPassword(password, username);
        }

        public ActionResult Logout()
        {
            try
            {
                Session[Constants.SessionItem.User] = null;
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Logger.Error("Cannot perform logout action", ex);
            }

            return RedirectToAction("Index");
        }
    }
}