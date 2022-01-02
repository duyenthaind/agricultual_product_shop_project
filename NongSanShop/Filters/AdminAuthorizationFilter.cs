using System;
using System.Web.Mvc;
using log4net;
using NongSanShop.Common;
using NongSanShop.Configuration.Attribute;
using NongSanShop.Models.Custom;

namespace NongSanShop.Filters
{
    public class AdminAuthorizationFilter : FilterAttribute, IActionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(AdminAuthorizationFilter));

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isSkipActionAttribute =
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipActionAttribute), true) ||
                filterContext.ActionDescriptor.IsDefined(typeof(SkipActionAttribute), true);
            if (isSkipActionAttribute)
            {
                return;
            }

            var currentUser = filterContext.HttpContext.Session[Constants.SessionItem.User];

            if (!(currentUser is AppUser appUser))
            {
                filterContext.Result = new RedirectResult("/Admin/Login");
                return;
            }

            if (appUser.AppRole.IsAdmin.Equals(false))
            {
                // change to unauthorized page and send status 403 later
                filterContext.Controller.ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào url này";
                filterContext.Result = new RedirectResult("/Admin/Login");
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var currentUser = filterContext.HttpContext.Session[Constants.SessionItem.User];
                if (currentUser is AppUser user)
                {
                    Logger.Debug(
                        $"user with username {user.Username} access action {filterContext.Controller.GetType()}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("On executed action error admin authorization filter error ", ex);
            }
        }
    }
}