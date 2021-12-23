using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongSanShop.Models;

namespace NongSanShop.Controllers
{
    public class HomeController : Controller
    {
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
    }
}