using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using NongSanShop.Common;
using NongSanShop.CustomException;
using NongSanShop.Models;
using NongSanShop.Models.Custom.Builder;
using NongSanShop.Util;
using PagedList;

namespace NongSanShop.Controllers
{
    public class HomeController : Controller
    {

        private static readonly ILog Logger = LogManager.GetLogger(nameof(HomeController));
        
        private const int numNewestProducts = 8; 
        
        private NongSanDB dbContext = new NongSanDB();
        public ActionResult Index()
        {
            var listCategories = dbContext.dh_category.ToList();
            var listTopProducts =
                dbContext.dh_product.OrderByDescending(p => p.created).Take(numNewestProducts).ToList();
            ViewBag.Categories = listCategories;
            ViewBag.Products = listTopProducts;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Blog(int? page)
        {

            var dh_blog = dbContext.dh_blog.Select(d => d);
            dh_blog = dh_blog.OrderBy(d => d.id);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(dh_blog.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Store(int? id, int? page)
        {
            Logger.Info($"Current id {id}");
            
            ViewBag.Categories = dbContext.dh_category.Select(p => p).ToList();
            var newestProducts = dbContext.dh_product.Take(8).ToList();
            if (id != null)
            {
                
                int categoryID = id.GetValueOrDefault();
                var products = dbContext.dh_product.Where(p => p.category_id == categoryID);
                
                if (products != null && products.ToList().Count > 0)
                {
                    Logger.Info(products);
                    newestProducts = products.Take(8).ToList();
                }else
                {
                    newestProducts = new List<dh_product>();
                }
            }

            //ViewBag.NewestProduct = newestProducts;

            
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(newestProducts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Login()
        {
            return View();
        }

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
                Logger.Error("Login error ", ex);
            }

            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            try
            {
                Session[Constants.SessionItem.User] = null;
            }
            catch (Exception ex)
            {
                Logger.Error("Perform action logout error ", ex);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ProductDetail(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var product = dbContext.dh_product.Find(id);
                if(product == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(product);
                }
            }
        }
    }
}