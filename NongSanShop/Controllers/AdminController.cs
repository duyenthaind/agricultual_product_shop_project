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
            return View();
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
    }
}