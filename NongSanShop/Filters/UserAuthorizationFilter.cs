using System;
using System.Web.Mvc;
using log4net;
using NongSanShop.Common;
using NongSanShop.Configuration.Attribute;
using NongSanShop.Models.Custom;

namespace NongSanShop.Filters
{
    public class UserAuthorizationFilter : FilterAttribute, IActionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(UserAuthorizationFilter));

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

            if (!(currentUser is AppUser))
            {
                filterContext.Result = new RedirectResult("/Home/Login");
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